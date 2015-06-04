using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Internal;
using DotLiquid;

namespace HairBand.Web
{
    internal class HairBandView : IView
    {

        //private ControllerContext _controllerContext;

        //public string MasterPath { get; protected set; }
        //public string ViewPath { get; protected set; }

        //public DotLiquidView(ControllerContext controllerContext,
        //    string partialPath)
        //    : this(controllerContext, partialPath, null)
        //{ }

        //public DotLiquidView(ControllerContext controllerContext,
        //    string viewPath, string masterPath)
        //{
        //    if (controllerContext == null)
        //        throw new ArgumentNullException("controllerContext");

        //    if (string.IsNullOrEmpty(viewPath))
        //        throw new ArgumentNullException("viewPath");

        //    _controllerContext = controllerContext;

        //    ViewPath = viewPath;
        //    MasterPath = masterPath;
        //}


        public HairBandView(string path)
        {
            this.Path = path;
        }
        public string Path { get; private set; }

        public async Task RenderAsync(ViewContext context)
        {

           // await context.Writer.WriteAsync("Hair band!!");

            if (context == null)
                throw new ArgumentNullException("viewContext");

            // Copy data from the view context over to DotLiquid
            var localVars = new Hash();

            if (context.ViewData.Model != null)
            {
                var model = context.ViewData.Model;

                //if(model.GetType().Name.EndsWith("ViewModel"))
                //{
                //    // If it's view model, just copy all properties to the localVars collection
                //    localVars.Merge(Hash.FromAnonymousObject(model));
                //}
                //else
                //{
                // It it's not a view model, just add the model direct as a "model" variable
                localVars.Add("model", model);
                //}
            }

            foreach (var item in context.ViewData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            foreach (var item in context.TempData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            var renderParams = new RenderParameters
            {
                LocalVariables = Hash.FromDictionary(localVars)
            };

            // Render the template
            var fileContents = ""; // VirtualPathProviderHelper.Load(ViewPath);
            var template = Template.Parse(fileContents);
            template.Render(context.Writer, renderParams);

        }
    }
}