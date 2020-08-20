using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using WasteManagement.Extensions;
using WasteManagement.Models.Interfaces;
using WasteManagement.Models.Models;

namespace WasteManagement.Controllers
{
    namespace AdSite.Controllers
    {
        //[Authorize(Roles = "Admin")]
        public class WasteContainerController : Controller
        {
            private readonly IWasteContainerService _wasteContainerService;
            private readonly ILogger<WasteContainerController> _logger;

            private const int SERVER_ERROR_CODE = 500;

            private string LOCALIZATION_SUCCESS_DEFAULT => "SuccessMessage_Default";
            private string LOCALIZATION_WARNING_INVALID_MODELSTATE => "WarningMessage_ModelStateInvalid";
            private string LOCALIZATION_ERROR_DEFAULT => "ErrorMessage_Default";
            private string LOCALIZATION_ERROR_USER_MUST_LOGIN => "ErrorMessage_MustLogin";
            private string LOCALIZATION_ERROR_NOT_FOUND => "ErrorMessage_NotFound";
            private string LOCALIZATION_ERROR_CONCURENT_EDIT => "ErrorMessage_ConcurrentEdit";

            public WasteContainerController(IWasteContainerService wasteContainerService, ILogger<WasteContainerController> logger)
            {
                _wasteContainerService = wasteContainerService;
                _logger = logger;
            }
            public IActionResult Details(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                try
                {
                    return View(_wasteContainerService.GetWasteContainerAsViewModel((Guid)id));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return NotFound(ex.Message).WithError(ex.Message);
                }
            }

            public IActionResult Index(string columnName, string searchString)
            {
                searchString = String.IsNullOrEmpty(searchString) ? String.Empty : searchString;
                columnName = String.IsNullOrEmpty(columnName) ? String.Empty : columnName;
                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentColumn"] = columnName;

                try
                {
                    return View(_wasteContainerService.GetWasteContainers(columnName, searchString));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return NotFound(ex.Message);
                }

            }
            // GET: WasteContainer/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: WasteContainer/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create([FromForm]WasteContainerCreateModel entity)
            {
                if (ModelState.IsValid)
                {
                    //TODO: string currentUser = HttpContext?.User?.Identity?.Name;
                    string currentUser = "WasteManagement";

                    if (!String.IsNullOrEmpty(currentUser))
                    {
                        try
                        {
                            //AuditedEntityMapper<WasteContainerCreateModel>.FillCreateAuditedEntityFields(entity, currentUser);

                            bool statusResult = _wasteContainerService.Add(entity);
                            if (statusResult)
                            {
                                return RedirectToAction(nameof(Index)).WithSuccess(LOCALIZATION_SUCCESS_DEFAULT);
                            }
                            else
                            {
                                return RedirectToAction(nameof(Index)).WithError(LOCALIZATION_ERROR_DEFAULT);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            return RedirectToAction(nameof(Index)).WithError(ex.Message);
                        }
                    }
                    else
                    {
                        _logger.LogError(LOCALIZATION_ERROR_USER_MUST_LOGIN);
                        return NotFound().WithError(LOCALIZATION_ERROR_USER_MUST_LOGIN);
                    }
                }
                return View(entity).WithWarning(LOCALIZATION_WARNING_INVALID_MODELSTATE);
            }


            // GET: WasteContainers/Edit/Guid
            public IActionResult Edit(Guid? id)
            {
                if (id == null)
                {
                    return NotFound().WithError(LOCALIZATION_ERROR_NOT_FOUND);
                }

                var wasteContainer = _wasteContainerService.GetWasteContainerAsEditModel((Guid)id);
                if (wasteContainer == null)
                {
                    return NotFound().WithError(LOCALIZATION_ERROR_NOT_FOUND);
                }

                return View(wasteContainer);
            }

            // POST: WasteContainers/Edit
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit([FromForm]WasteContainerUpdateModel entity)
            {
                if (ModelState.IsValid)
                {
                    //TODO: string currentUser = HttpContext?.User?.Identity?.Name;
                    string currentUser = "WasteManagement";
                    if (!String.IsNullOrEmpty(currentUser))
                    {
                        //AuditedEntityMapper<WasteContainerEditModel>.FillModifyAuditedEntityFields(entity, currentUser);

                        try
                        {
                            _wasteContainerService.Update(entity);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!WasteContainerExists(entity.Id))
                            {
                                _logger.LogError(LOCALIZATION_ERROR_NOT_FOUND);
                                return NotFound().WithError(LOCALIZATION_ERROR_NOT_FOUND);
                            }
                            else
                            {
                                _logger.LogError(LOCALIZATION_ERROR_CONCURENT_EDIT);
                                return NotFound().WithError(LOCALIZATION_ERROR_CONCURENT_EDIT);
                            }
                        }

                        return RedirectToAction(nameof(Details), new { id = entity.Id }).WithSuccess(LOCALIZATION_SUCCESS_DEFAULT);
                    }
                    else
                    {
                        _logger.LogError(LOCALIZATION_ERROR_USER_MUST_LOGIN);
                        return NotFound().WithError(LOCALIZATION_ERROR_USER_MUST_LOGIN);
                    }
                }
                return View(entity).WithWarning(LOCALIZATION_WARNING_INVALID_MODELSTATE);
            }


            // GET: WasteContainer/Delete/5
            public IActionResult Delete(Guid? id)
            {
                if (id == null)
                {
                    return NotFound().WithError(LOCALIZATION_ERROR_NOT_FOUND);
                }

                var wasteContainer = _wasteContainerService.GetWasteContainerAsViewModel((Guid)id);
                if (wasteContainer == null)
                {
                    return NotFound().WithError(LOCALIZATION_ERROR_NOT_FOUND);
                }

                return View(wasteContainer);
            }


            // POST: WasteContainer/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(Guid id)
            {
                if (id == null)
                {
                    return NotFound().WithError(LOCALIZATION_ERROR_NOT_FOUND);
                }

                try
                {
                    _wasteContainerService.Delete(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return RedirectToAction(nameof(Index)).WithError(ex.Message);
                }

                return RedirectToAction(nameof(Index)).WithSuccess(LOCALIZATION_SUCCESS_DEFAULT);
            }

            private bool WasteContainerExists(Guid id)
            {
                return _wasteContainerService.Exists(id);
            }
        }
    }
}
