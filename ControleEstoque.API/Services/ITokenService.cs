using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ControleEstoque.API.Data;



namespace ControleEstoque.API.Services
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);


    }
}
