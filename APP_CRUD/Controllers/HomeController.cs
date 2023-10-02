using APP_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using APP_CRUD.Repositorios.Contrato;

namespace APP_CRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IGenericRepository<Departamento> _departamentoRepository;
        private readonly IGenericRepository<Personal> _personalRepository;

        public HomeController(ILogger<HomeController> logger, 
            IGenericRepository<Departamento> departamentoRepository, 
            IGenericRepository<Personal> personalRepository)
        {
            _logger = logger;
            _departamentoRepository = departamentoRepository;
            _personalRepository = personalRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> listaDepartamentos()
        {
            List<Departamento> _lista = await _departamentoRepository.Listar();
            return StatusCode(StatusCodes.Status200OK,_lista);
        }

        [HttpGet]
        public async Task<IActionResult> listaPersonal()
        {
            List<Personal> _lista = await _personalRepository.Listar();
            return StatusCode(StatusCodes.Status200OK, _lista);
        }

        [HttpPost]
        public async Task<IActionResult> guardarPersonal([FromBody] Personal modelo)
        {
            bool _resultado = await _personalRepository.Guardar(modelo);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "Ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }
        [HttpPut]
        public async Task<IActionResult> editarPersonal([FromBody] Personal modelo)
        {
            bool _resultado = await _personalRepository.Editar(modelo);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "Ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }

        [HttpPut]
        public async Task<IActionResult> eliminarPersonal(int idPersonal)
        {
            bool _resultado = await _personalRepository.Eliminar(idPersonal);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "Ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}