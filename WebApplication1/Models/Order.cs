using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Stateless;

namespace WebApplication1.Models
{
    public enum OrderStateEnum
    {
        Pending,     // 订单已生成
        Processing,      // 支付处理中
        Completed,       // 交易成功
        Declined,        // 交易失败
        Cancelled,       // 订单取消
        Refunded         // 已退款
    }

    public enum OrderStateTriggerEnum
    {
        PlaceOrder,   // 支付
        Approve,      // 收款成功
        Reject,       // 收款失败
        Cancel,      // 取消
        Return       // 退货
    }

    public class Order
    {
        public Order()
        {
            StateMachineInit();
        }
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Id")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
        public OrderStateEnum State { get; set; }
        public DateTime CreateTime { get; set; }
        public string TransactionMetadata { get; set; }       // 第三方支付数据
        private StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine;


        public void PaymentProcessing()
        {
            _machine.Fire(OrderStateTriggerEnum.PlaceOrder);
        }

        public void PaymentApprove()
        {
            _machine.Fire(OrderStateTriggerEnum.Approve);
        }

        public void PaymentReject()
        {
            _machine.Fire(OrderStateTriggerEnum.Reject);
        }

        private void StateMachineInit()
        {
            // _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(OrderStateEnum.Pending);

            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(
                () => State,
                s => State = s
            );

            _machine.Configure(OrderStateEnum.Pending)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);

            _machine.Configure(OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);

            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refunded);

        }

    }
}
