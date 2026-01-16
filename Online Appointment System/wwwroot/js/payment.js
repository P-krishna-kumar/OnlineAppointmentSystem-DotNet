$(document).ready(function () {

    $("#btnPay").click(function () {

        let data = {
            ServiceId: $("#ServiceId").val(),
            TimeSlotId: $("#TimeSlotId").val(),
            AppointmentDate: $("#AppointmentDate").val()
        };

        if (data.ServiceId === "" || data.TimeSlotId === "" || data.AppointmentDate === "") {
            alert("All fields required!");
            return;
        }

        $.ajax({
            url: "/Appointment/CreateOrder",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function (res) {

                var options = {
                    "key": res.key,
                    "amount": res.amount,
                    "currency": "INR",
                    "name": "Appointment Payment",
                    "order_id": res.orderId,
                    "handler": function (response) {

                        var successData = {
                            paymentId: response.razorpay_payment_id,
                            orderId: response.razorpay_order_id,
                            serviceId: data.ServiceId,
                            timeSlotId: data.TimeSlotId,
                            date: data.AppointmentDate
                        };

                        $.ajax({
                            url: "/Appointment/PaymentSuccess",
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify(successData),
                            success: function (msg) {
                                alert(msg.message);
                                window.location.href = "/Appointment/MyAppointments";
                            }
                        });
                    }
                };

                var rzp = new Razorpay(options);
                rzp.open();
            }
        });
    });
});
