using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Backend.Api.Validation
{
    public class ModelStateValidatorConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controllerModel in application.Controllers)
            {
                controllerModel.Filters.Add(new ModelStateValidationFilterAttribute());
            }
        }
    }
}
