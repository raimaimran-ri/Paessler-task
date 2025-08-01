using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using MediatR;
using Paessler.Task.Services.Handlers.Commands;

namespace Paessler.Task.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                var order = await _mediator.Send(new GetOrderByIdCommand { OrderId = id });
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            try
            {
                // Logic to create an order
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}