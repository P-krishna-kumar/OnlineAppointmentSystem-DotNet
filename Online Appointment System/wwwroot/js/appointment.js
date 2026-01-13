$(document).ready(function () {

    $("#btnBook").click(function () {
        let obj = {
            ServiceId: $("#ServiceId").val(),
            TimeSlotId: $("#TimeSlotId").val(),
            AppointmentDate: $("#AppointmentDate").val()
        };

        if (obj.ServiceId === "" || obj.TimeSlotId === "" || obj.AppointmentDate === "") {
            $("#msg").html('<div class="alert alert-danger">All fields are required!</div>');
            return;
        }

        $("#msg").html('<div class="alert alert-info">Processing...</div>');

        $.ajax({
            url: "/Appointment/AjaxCreate",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(obj),
            success: function (res) {

                if (res.status === true) {
                    $("#msg").html('<div class="alert alert-success">' + res.message + '</div>');

                    $("#ServiceId").val("");
                    $("#TimeSlotId").val("");
                    $("#AppointmentDate").val("");
                }
                else if (res.message === "Not Logged In") {
                    window.location.href = "/Account/Login";
                }
                else {
                    $("#msg").html('<div class="alert alert-danger">' + res.message + '</div>');
                }
            },
            error: function () {
                $("#msg").html('<div class="alert alert-danger">Something went wrong!</div>');
            }
        });

    });
});






