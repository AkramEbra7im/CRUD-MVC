using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (SearchValue == null)
            {
                var Roles = await _roleManager.Roles.ToListAsync();
                var MappedRoles = _mapper.Map<IEnumerable<RoleViewModel>>(Roles);
                return View(MappedRoles);
            }
            else
            {
                var Roles = await _roleManager.FindByNameAsync(SearchValue);
                var MappedRole = _mapper.Map<RoleViewModel>(Roles);
                return View(new List<RoleViewModel> { MappedRole });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var MappedRole = _mapper.Map<IdentityRole>(model);
                await _roleManager.CreateAsync(MappedRole);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();

            var Role = await _roleManager.FindByIdAsync(id);

            if (Role is null)
                return NotFound();

            var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(ViewName, MappedRole);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model, [FromRoute] string id)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(id);
                    Role.Name = model.RoleName;
                    await _roleManager.UpdateAsync(Role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var Role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(Role);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
