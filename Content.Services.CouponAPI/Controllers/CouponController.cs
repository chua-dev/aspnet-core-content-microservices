using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Content.Services.CouponAPI.Data;
using Content.Services.CouponAPI.Models;
using Content.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Content.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _autoMapper;
        private ResponseDto _response;


        public CouponController(AppDbContext db, IMapper autoMapper)
        {
            _db = db;
            _response = new ResponseDto();
            _autoMapper = autoMapper;
        }

        [HttpGet]
        public ActionResult<ResponseDto> getAllCoupon()
        {
            IEnumerable<Coupon> couponList = _db.Coupons.ToList();
            if (couponList == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to get coupon list";
                return NotFound(_response);
            }

            IEnumerable<CouponDto> mappedCouponList = _autoMapper.Map<IEnumerable<CouponDto>>(couponList);
            _response.Result = mappedCouponList;
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<ResponseDto> getCouponById([FromRoute] int id)
        {
            Coupon coupon = _db.Coupons.FirstOrDefault(u => u.CouponId == id);
            

            if (coupon == null)
            {
                _response.IsSuccess = false;
                _response.Message = $"Fail to get coupon of id: {id}";
                return NotFound(_response);
            }

            CouponDto mappedCoupon = _autoMapper.Map<CouponDto>(coupon);
            _response.Result = mappedCoupon;
            return _response;
        }

        [HttpGet]
        [Route("{code}")]
        public ActionResult<ResponseDto> getCouponByCode([FromRoute] string code)
        {
            Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());

            if (coupon == null)
            {
                _response.IsSuccess = false;
                _response.Message = $"Fail to get coupon of id: {code}";
                return NotFound(_response);
            }

            _response.Result = coupon;
            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> createCoupon([FromBody] CouponDto coupon)
        {
            Coupon mappedCoupon = _autoMapper.Map<Coupon>(coupon);
            _db.Coupons.Add(mappedCoupon);
            int changes = await _db.SaveChangesAsync();

            if (changes > 0)
            {
                _response.Result = _autoMapper.Map<CouponDto>(mappedCoupon);
                return _response;
            }

            _response.IsSuccess = false;
            _response.Message = "Failed to create";
            return NotFound(_response);
        }

        [HttpPut]
        //[Route("{id:int}")]
        public async Task<ActionResult<ResponseDto>> editCoupon([FromBody] CouponDto coupon)
        {
            Coupon mappedCoupon = _autoMapper.Map<Coupon>(coupon);
            _db.Coupons.Update(mappedCoupon);
            int changes = await _db.SaveChangesAsync();

            if (changes > 0)
            {
                CouponDto returnCoupon = _autoMapper.Map<CouponDto>(mappedCoupon);
                _response.Result = returnCoupon;
                return _response;
            }

            _response.IsSuccess = false;
            _response.Message = "Failed to update";
            return NotFound(_response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<ResponseDto>> deleteCoupon(int id)
        {
            Coupon coupon = _db.Coupons.FirstOrDefault(c => c.CouponId == id);

            if (coupon == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to delete";
                return NotFound(_response);
            }

            _db.Coupons.Remove(coupon);
            _db.SaveChanges();

            _response.Result = coupon;
            _response.Message = "Successfully Deleted";
            return _response;
        }
    }
}

