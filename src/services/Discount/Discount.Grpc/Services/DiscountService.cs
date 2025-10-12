using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<GetDiscountsResponse> GetDiscounts(EmptyRequest request, ServerCallContext context)
        {
            var response = new GetDiscountsResponse();
            response.Response.AddRange(await dbContext.Coupons.ProjectToType<CouponModel>().ToListAsync());
            return response;
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName.ToLower() == request.ProductName.ToLower());

            coupon ??= new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc." };

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>() 
                ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

            var couponAlreadyExists = await dbContext.Coupons.AnyAsync(c => c.ProductName.ToLower() == request.Coupon.ProductName.ToLower());
            
            if (couponAlreadyExists)
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Discount already exists"));

            await dbContext.Coupons.AddAsync(coupon);
            await dbContext.SaveChangesAsync();

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>()
                ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

            var couponExists = await dbContext.Coupons.AsNoTracking().AnyAsync(c => c.Id == request.Coupon.Id);
            if(!couponExists) 
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount Not Found"));

            dbContext.Update(coupon);
            await dbContext.SaveChangesAsync();
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName.ToLower() == request.ProductName.ToLower())
                ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount Not Found"));

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            return new DeleteDiscountResponse { Success = true };
        }
    }
}
