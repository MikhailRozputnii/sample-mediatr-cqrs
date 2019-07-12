using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagement.Commands;
using UserManagement.Common.Exceptions;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    /// <summary>
    /// This class used to bind user data
    /// </summary>
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        public UserController(IMapper mapper, IMediator mediator, ILoggerFactory logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger.CreateLogger(nameof(UserController));
        }
        /// <summary>
        /// This is the method for sending a request to getting data 
        /// </summary>
        /// <param name="model">includes basic properties and operations List</param>
        /// <returns>data to view pages</returns>
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (model.CurrentPage <= 0 || model.Search != null)
            {
                _logger.LogInformation("Getting users");
                model.CurrentPage = 1;
            }
            var result = await _mediator.Send(new GetUsers
            {
                Search = model.Search,
                CurrentPage = model.CurrentPage,
                PageSize = model.PageSize,
                SortField = model.SortOrder,
                IsAscending = model.IsAscending
            });

            var users = result.Item1;
            int countUsers = result.Item2;

            if (users.Count() == 0)
            {
                _logger.LogWarning("User not found");
                model.Message = MessagesFields.NotFound;
                return View(model);
            }
            model.Users = _mapper.Map<IEnumerable<UserViewModel>>(users);
            model.PageInfo = new PageInfo(countUsers, model.PageSize, model.CurrentPage);

            return View(model);
        }

        /// <summary>
        /// This is the method for sending a request to view pages
        /// </summary>
        /// <returns>empty action method</returns>
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation($"Request to creating user");
            return View();
        }

        /// <summary>
        /// This is the method for sending a request to server for adding data
        /// </summary>
        /// <param name="model"> basic user's  properties</param>
        /// <returns>redirect action to view index pages</returns>
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            _logger.LogInformation($"Creating user {model.Email}");
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new CreateUser { FirstName = model.FirstName, Email = model.Email, LastName = model.LastName });
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException ex) {
                    return View(new UserViewModel { Message = ex.Message });
                }
                catch (Exception ex) {
                    _logger.LogWarning($"Unknown error: {ex.Message}. Trace {ex.StackTrace}");
                    return View(new UserViewModel { Message = MessagesFields.BadRequest });
                }
            }
            _logger.LogWarning($"Data {ModelState} is not valid");
            return View();
        }


        /// <summary>
        /// This is the method for sending a request to server for getting data
        /// </summary>
        /// <param name="id">user id: Guid object/param>
        /// <returns>request to view pages</returns>
        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            _logger.LogInformation($"Removing user {id}");
            if (!id.Equals(Guid.Empty))
            {
                var user = await _mediator.Send(new GetUser { Id = id });
                if (user != null)
                {
                    var viewModel = _mapper.Map<UserViewModel>(user);
                    return View(viewModel);
                }
                else
                {
                    _logger.LogWarning($"User by id ({id}) not found");
                    return View(new UserViewModel { Message = MessagesFields.NotFound });
                }
            }
            _logger.LogWarning($"Error in Id {id}");
            return View(new UserViewModel { Message = MessagesFields.BadRequest });
        }

        /// <summary>
        /// This is the method for sending a request to server for removing data
        /// </summary>
        /// <param name="id">user id: Guid object</param>
        /// <returns>redirect action to view index pages</returns>
        [HttpPost]
        public async Task<IActionResult> СonfirmRemove(Guid id)
        {
            _logger.LogInformation($"Confirm removing user {id}");
            if (!id.Equals(Guid.Empty))
            {
                try
                {
                    await _mediator.Send(new RemoveUser { Id = id });
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    return View(new UserViewModel { Message = ex.Message });
                }
                catch (Exception ex) {
                    _logger.LogWarning($"Unknown error: {ex.Message}. Trace {ex.StackTrace}");
                    return View(new UserViewModel { Message = MessagesFields.BadRequest });
                }
            }
            _logger.LogWarning($"Error in Id {id}");
            return View(new UserViewModel { Message = MessagesFields.BadRequest });
        }

        /// <summary>
        /// This is the method for sending a request to server for getting data
        /// </summary>
        /// <param name="id">user id: Guid object</param>
        /// <returns>request to view pages</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            _logger.LogInformation($"Updating user {id}");
            if (!id.Equals(Guid.Empty))
            {
                try
                {
                    var user = await _mediator.Send(new GetUser { Id = id });
                    if (user != null)
                    {
                        var viewModel = _mapper.Map<UserViewModel>(user);
                        return View(viewModel);
                    }
                    else
                    {
                        _logger.LogWarning($"User by id ({id}) not found");
                        return View(new UserViewModel { Message = MessagesFields.NotFound });
                    }
                }
                catch (InvalidOperationException ex) {
                   return View(new UserViewModel { Message = ex.Message });
                }catch (Exception ex)
                {
                    _logger.LogWarning($"Unknown error: {ex.Message}. Trace {ex.StackTrace}");
                    return View(new UserViewModel { Message = MessagesFields.BadRequest });
                }

            }
            _logger.LogWarning($"Error in Id {id}");
            return View(new UserViewModel { Message = MessagesFields.BadRequest });
        }

        /// <summary>
        /// This is the method for sending a request to server for update data
        /// </summary>
        /// <param name="viewModel">object properties to update data</param>
        /// <returns>redirect action to view index pages</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel viewModel)
        {
            _logger.LogInformation($"Updating user {viewModel.Id}");
            if (ModelState.IsValid)
            {
                if (!viewModel.Id.Equals(Guid.Empty))
                {
                    try{
                        await _mediator.Send(new UpdateUser
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Email = viewModel.Email,
                            Id = viewModel.Id
                        });
                        return RedirectToAction("Index");
                    }
                    catch (NotFoundException){
                        return View(new UserViewModel { Message = MessagesFields.NotFound });
                    }
                    catch (ModelValidationException) {
                        return View(new UserViewModel { Message = MessagesFields.UniqueEmail });
                    }
                    catch (InvalidOperationException ex)
                    {
                        return View(new UserViewModel { Message = ex.Message });
                    }
                    catch (Exception ex) {
                        _logger.LogWarning($"Unknown error: {ex.Message}. Trace {ex.StackTrace}");
                        return View(new UserViewModel { Message = MessagesFields.BadRequest });
                    }
                }
                _logger.LogWarning($"Error in Id {viewModel.Id}");
                return View(new UserViewModel { Message = MessagesFields.BadRequest });
            }
            _logger.LogWarning($"Data {ModelState} is not valid");
            return View();
        }

        /// <summary>
        /// This is the method for sending a request to server for getting data
        /// </summary>
        /// <param name="id">user id: Guid object</param>
        /// <returns>request to view data page</returns>
        public async Task<IActionResult> Detail(Guid id)
        {
            _logger.LogInformation($"Getting user {id}");
            if (!id.Equals(Guid.Empty))
            {
                try
                {
                    var user = await _mediator.Send(new GetUser { Id = id });
                    if (user != null)
                    {
                        var viewModel = _mapper.Map<UserViewModel>(user);
                        return View(viewModel);
                    }
                    else
                    {
                        _logger.LogWarning($"User by id({id}) not found");
                        return View(new UserViewModel { Message = MessagesFields.NotFound });
                    }
                }
                catch (InvalidOperationException ex)
                {
                    return View(new UserViewModel { Message = ex.Message });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Unknown error: {ex.Message}. Trace {ex.StackTrace}");
                    return View(new UserViewModel { Message = MessagesFields.BadRequest });
                }
            }
            else
            {
                _logger.LogWarning($"Error in Id {id}");
                return View(new UserViewModel { Message = MessagesFields.BadRequest });
            }
        }
    }
}