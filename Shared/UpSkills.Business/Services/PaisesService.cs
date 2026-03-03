using Microsoft.EntityFrameworkCore;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Models.DTO;
using System.Net;
using AutoMapper;

namespace UpSkills.Business.Services;

public class PaisesService (AppDbContext _AppDbContext, IMapper _Mapper) : IPaisesService
{
    public async Task<ApiResponse> GetAll()
    {
        try
        {
            var paises = await _AppDbContext.Pais.AsNoTracking()
                .ToListAsync();

            return new(
                statusCode: paises.Count != 0 ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: _Mapper.Map<List<PaisDTO>>(paises)
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new(
                statusCode: HttpStatusCode.InternalServerError,
                message: $"Error de consulta: {msg}."
            );
        }
    }
}
