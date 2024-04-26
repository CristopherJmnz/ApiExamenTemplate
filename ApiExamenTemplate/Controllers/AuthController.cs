using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ApiExamenTemplate.Models;
using ApiExamenTemplate.Helpers;
using ApiExamenTemplate.Repositories;
using System.IdentityModel.Tokens.Jwt;

namespace ApiExamenTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private DoctoresRepository repo;
        //CUANDO GENEREMOS EL TOKEN, DEBEMOS INTEGRAR
        //DENTRO DE DICHO TOKEN, ISSUER, AUDIENCE...
        //PARA QUE LO VALIDE CUANDO NOS LO ENVIEN.
        private HelperActionServicesOAuth helper;

        public AuthController(DoctoresRepository repo
            , HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        //NECESITAMOS UN METODO POST PARA VALIDAR EL 
        //USUARIO Y QUE RECIBIRA LoginModel
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            //BUSCAMOS AL EMPLEADO EN NUESTRO REPO
            Doctor doctor=
                await this.repo.LogInAsync
                (model.Username, model.Password);
            if (doctor == null)
            {
                return Unauthorized();
            }
            else
            {
                //DEBEMOS CREAR UNAS CREDENCIALES PARA 
                //INCLUIRLAS DENTRO DEL TOKEN Y QUE ESTARAN 
                //COMPUESTAS POR EL SECRET KEY CIFRADO Y EL TIPO
                //DE CIFRADO QUE DESEEMOS INCLUIR EN EL TOKEN
                SigningCredentials credentials =
                    new SigningCredentials(
                        this.helper.GetKeyToken()
                        , SecurityAlgorithms.HmacSha256);
                //EL TOKEN SE GENERA CON UNA CLASE Y 
                //DEBEMOS INDICAR LOS ELEMENTOS QUE ALMACENARA 
                //DENTRO DE DICHO TOKEN, POR EJEMPLO, ISSUER,
                //AUDIENCE O EL TIEMPO DE VALIDACION DEL TOKEN
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                //POR ULTIMO, DEVOLVEMOS UNA RESPUESTA AFIRMATIVA
                //CON UN OBJETO ANONIMO EN FORMATO JSON
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler()
                        .WriteToken(token)
                    });
            }
        }

    }
}
