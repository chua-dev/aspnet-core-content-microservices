using System;
using AutoMapper;
using Content.Services.CouponAPI.Models;
using Content.Services.CouponAPI.Models.Dto;

namespace Content.Services.CouponAPI
{
	public class MappingConfig
	{
		// Auto Mapper, if the property name are the same, it will auto map
		public static MapperConfiguration RegisterMaps()
		{
			MapperConfiguration mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Coupon, CouponDto>();
				config.CreateMap<CouponDto, Coupon>();
			});

			return mappingConfig;
		}
	}
}

