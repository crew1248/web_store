namespace x_nova_template.Binders
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class ArrayFilterBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if ((result == null) || string.IsNullOrEmpty(result.AttemptedValue))
            {
                return null;
            }
            return result.AttemptedValue.Split(new char[] { ',' }).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
        }
    }
}

