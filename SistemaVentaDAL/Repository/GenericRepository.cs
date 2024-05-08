using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repository.Contrato;
using System.Linq.Expressions;

namespace SistemaVenta.DAL.Repository
{
    public class GenericRepository<TModelo>: IGenericRepository<TModelo> where TModelo : class
    {
        private readonly DbventaContext _dbcontext;

        public GenericRepository(DbventaContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                TModelo modelo = await _dbcontext.Set<TModelo>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch
            {
                throw;
            }
        }
        public async Task<TModelo> Crear(TModelo model)
        {
            try
            {
                _dbcontext.Set<TModelo>().Add(model);
                await _dbcontext.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TModelo model)
        {
            try
            {
               _dbcontext.Set<TModelo>().Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {

                throw;
            }
        }
        public async Task<bool> Eliminar(TModelo model)
        {
            try
            {
                _dbcontext.Set<TModelo>().Remove(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {

                throw;
            }
        }

        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> queryModel = filtro == null ? _dbcontext.Set<TModelo>() : _dbcontext.Set<TModelo>().Where(filtro);
                return queryModel;
            }
            catch
            {

                throw;
            }
        }
    }
}
