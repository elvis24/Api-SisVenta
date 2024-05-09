using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Guardar([FromBody] VentaDTO ventaDTO)
        {
            var rsp = new Response<VentaDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _ventaService.Registrar(ventaDTO);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor, string? numVenta, string?fechaIni, string? fechaFin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numVenta = numVenta is null ? "" : numVenta;
            fechaIni = fechaIni is null ? "" : fechaIni;
            fechaFin = fechaFin is null ? "" : fechaFin;

            try
            {
                rsp.status = true;
                rsp.value = await _ventaService.Historial(buscarPor,numVenta,fechaIni,fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte( string? fechaIni, string? fechaFin)
        {
            var rsp = new Response<List<ReporteDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _ventaService.Reporte(fechaIni, fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }
    }
}
