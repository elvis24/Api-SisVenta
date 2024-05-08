using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repository.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class VentaService:IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleVentaRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaGenerda = await _ventaRepository.Registrar(_mapper.Map<Venta>(modelo));

                if(ventaGenerda.IdVenta == 0)
                    throw new TaskCanceledException("Np se pudo crear");
                return _mapper.Map<VentaDTO>(ventaGenerda);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar();
            var ListaResultado = new List<Venta>();
            try
            {
                if (buscarPor =="fecha")
                {
                    DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                    ListaResultado = await query.Where(x =>
                    x.FechaRegistro.Value.Date >= fech_inicio.Date &&
                    x.FechaRegistro.Value.Date <= fech_fin.Date).Include(dv => dv.DetalleVenta).ThenInclude(p => p.IdProductoNavigation).ToListAsync();
                }
                else
                {
                    ListaResultado = await query.Where(x =>
                    x.NumeroDocumento == numeroVenta).Include(dv => dv.DetalleVenta).ThenInclude(p => p.IdProductoNavigation).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _mapper.Map<List<VentaDTO>>(ListaResultado);
        }



        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
            var ListaResulado = new List<DetalleVenta>();
            try
            {
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                ListaResulado = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value >= fech_inicio.Date &&
                           dv.IdVentaNavigation.FechaRegistro.Value <= fech_fin.Date).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(ListaResulado);
        }
    }
}
