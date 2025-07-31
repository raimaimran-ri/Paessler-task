using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Paessler.Task.Model;
using Paessler.Task.Model.Models;

namespace Paessler.Task.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;

        public OrderController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                
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
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}