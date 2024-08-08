using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerExcludeAuthFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Excluir autorización para AuthController
        var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        if (controllerActionDescriptor != null && controllerActionDescriptor.ControllerName == "Auth")
        {
            operation.Security.Clear(); // Eliminar requisitos de seguridad
        }
    }
}
