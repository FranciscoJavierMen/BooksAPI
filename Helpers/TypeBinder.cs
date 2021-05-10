using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var valueprovider = bindingContext.ValueProvider.GetValue(propertyName);

            if(valueprovider == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var deserializedValue = JsonConvert.DeserializeObject<T>(valueprovider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializedValue);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(propertyName, "Invalid type value for List<int>");
            }
            return Task.CompletedTask;
        }
    }
}
