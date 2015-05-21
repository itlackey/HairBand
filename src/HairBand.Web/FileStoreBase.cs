using Microsoft.AspNet.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand.Web
{
    public abstract class FileStoreBase<TItem, TKey> : IDisposable where TItem : IItem<TKey>
    {

        private IHostingEnvironment _host;
        private string _rootPath;
        private List<TItem> _items = new List<TItem>();

        public List<TItem> Items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = value;
            }
        }

        public FileStoreBase(IHostingEnvironment host, string relativeRootPath)
        {
            _host = host;
            _rootPath = _host.MapPath("app_data/" + relativeRootPath);

            if (!Directory.Exists(_rootPath))
                Directory.CreateDirectory(_rootPath);
        }


        #region Get
        protected virtual string GetFileName(TItem item)
        {
            return string.Format("{0}#{1}", item.Id, item.Name);
        }

        protected string GetFilePath(TItem item)
        {
            return Path.Combine(_rootPath, GetFileName(item));
        }

        protected virtual async Task<TItem> GetItemById(string id)
        {
            TItem item = default(TItem);

            var itemPath = GetFilePathFromId(id);

            if (item == null && File.Exists(itemPath))
            {
                var data = File.ReadAllText(itemPath);

                item = JsonConvert.DeserializeObject<TItem>(data);

                if (item != null)
                    Items.Add(item);
            }

            return item;
        }

        protected Task<TItem> GetItemByName(string name)
        {
            var item = Items.FirstOrDefault(u => u.Name == name);

            var userPath = GetFilePathFromUsername(name);

            if (item == null && File.Exists(userPath))
            {
                var data = File.ReadAllText(userPath);

                item = JsonConvert.DeserializeObject<TItem>(data);

                if (item != null)
                    Items.Add(item);
            }
            return Task.FromResult(item);
        }

        private string GetFilePathFromId(string id)
        {
            var files = Directory.GetFiles(_rootPath, id + "#*");

            if (files.Count() == 1)
                return Path.Combine(_rootPath, files.First());
            else
                return string.Empty;

        }
        private string GetFilePathFromUsername(string username)
        {
            var files = Directory.GetFiles(_rootPath, "*#" + username);

            if (files.Count() == 1)
                return Path.Combine(_rootPath, files.First());
            else
                return string.Empty;
        }

        protected virtual IEnumerable<TItem> GetItems()
        {
            var files = Directory.GetFiles(_rootPath);

            foreach (var file in files)
            {
                var data = File.ReadAllText(Path.Combine(_rootPath, file));

                yield return JsonConvert.DeserializeObject<TItem>(data);

            }

        }

        #endregion




        protected async Task<TItem> CreateAsync(TItem item)
        {
            if (this.Items.Contains(item))
                throw new InvalidOperationException("Item already exists");

            Items.Add(item);

            await SaveItemAsync(item);

            return item;
        }

        protected Task DeleteAsync(TItem item)
        {
            return Task.Run(() =>
            {
                if (Items.Contains(item))
                    Items.Remove(item);

                var path = GetFilePath(item);

                if (File.Exists(path))
                    File.Delete(GetFilePath(item));
                else
                    throw new KeyNotFoundException();

            });
        }

        protected virtual Task SaveItemAsync(TItem item)
        {
            return Task.Run(() =>
            {
                var data = JsonConvert.SerializeObject(item);

                File.WriteAllText(GetFilePath(item), data);

            });

        }

        protected virtual string NormalizeName(string name)
        {

            //ToDo make UNC safe
            return name;
        }


        public void Dispose()
        {

        }

    }
}
