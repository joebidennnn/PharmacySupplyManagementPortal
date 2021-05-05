$(".repSchedule").on("click", function () {
    $("#RepSchedule").show()
    $("#medicineDemand").hide()
    $("#mainPage").hide()
})


$(".medicineDemand").on("click", function () {
    $("#RepSchedule").hide();
    $("#medicineDemand").show()
    $("#mainPage").hide()
})

$(".mainPage").on("click", function () {
    $("#RepSchedule").hide();
    $("#medicineDemand").hide()
    $("#mainPage").show()
})

$(".nav-link").on("click", function () {
    $(".active").removeClass("active")
    $(this).addClass("active")
})


$('.input-date-rep').on("change", function (e) {
    e.preventDefault();
    $(".spinner").show();
    $(".repMessage").text("")
    $.ajax({
        type: "GET",
        url: "../RepSchedule/Schedule",
        data: $("#repScheduleForm").serialize(),
        success: function (data) {
            $(".spinner").hide();
            console.log(data)
            if (typeof data == "string") {
                if (data == "Unauthorized") {
                    window.location.reload()
                }
                $(".first").hide()
                $(".repMessage").text(data)
            }
            else {
                var raw = ""
                data.forEach(function (i) {
                    var rawDate = String(new Date(i["dateOfMetting"])).substring(0, 15)
                    var li = rawDate.split(" ")
                    var date = `${li[2]}-${li[1]}-${li[3]}`
                    raw += "<tr><td>" + i["name"] + "</td><td>" + i["docterName"] + "</td><td>" + i["treatmentAilment"] + "</td><td>" + i["medicine"] + "</td><td>" + i["mettingSlot"] + "</td><td>" + date + "</td><td>" + i["docterContactNumber"] + "</td></tr>"
                })
                $("#repTableBody").html(raw)
                $(".first").show()
            }

        }
    })
})
if (performance.navigation.type == 2) {
    location.reload(true);
}


$(".view-demand").on("click", function () {
    $(".result").text("")
    $(".mdResult").text("")
    $(".view-demand").hide()
    $(".spinner-three").show()
    $.ajax({
        type: "GET",
        url: "../MedicineDemandSupply/Index",
        success: function (data) {
            $(".view-demand").show()
            $(".spinner-three").hide()
            console.log(data)
            if (typeof data == "string") {
                $(".result").text(data)
                $(".demandTable").hide()
                $(".demandTableTwo").hide()
            }
            else {

                var raw = ""
                data.forEach(function (i) {
                    raw = raw + "<tr><td><input type='text' name='medicine'  class='form-control medicineName' value='" + i["medicine"] + "' readonly></td><td><input type='number' min='0' class='form-control medicineCount' name='demandCount' value='" + i["demandCount"] + "'/></td>";

                })
                raw = raw + "";
                $(".demandTableBody").html(raw)
                $(".demandTable").show()
                $(".demandTableTwo").hide()
            }

        }
    })
})

function demadMedicine() {
    $(".demandMedicine-btn").hide()
    $(".spinner-one").show()
    $(".mdResult").text("")
    var names = []
    var count = []
    $(".medicineName").each(function () {
        names.push($(this).val())
    })
    $(".medicineCount").each(function () {
        count.push($(this).val())
    })
    var li = []

    for (var i = 0; i < names.length; i++) {
        var temp = {}
        temp["demandCount"] = count[i]
        temp["medicine"] = names[i]
        li.push(temp)
    }

    $.ajax({
        type: "POST",
        url: "../MedicineDemandSupply/Index",
        data: { demands: li },
        success: function (data) {
            $(".demandMedicine-btn").show()
            $(".spinner-one").hide()
            console.log(data)
            if (typeof data == "string") {
                $(".mdResult").text(data)
                $(".demandTable").hide()
                $(".demandTableTwo").hide()
            }
            else {
                console.log(data)
                listCount = {}
                var raw = ""
                data.forEach(function (i) {
                    raw = raw + "<tr><td>" + i["pharmacyName"] + "</td><td>" + i["medicineName"] + "</td><td>" + i["supplyCount"] + "</td></tr>";
                    if (listCount[i["medicineName"]] === undefined) {
                        listCount[i["medicineName"]] = parseInt(i["supplyCount"])
                    }
                    else {
                        listCount[i["medicineName"]] = parseInt(listCount[i["medicineName"]]) + parseInt(i["supplyCount"])
                    }
                })
                var mName = ""
                var f = 0
                li.forEach(function (temp) {
                    if (temp["demandCount"] > listCount[temp["medicine"]]) {
                        mName += temp["medicine"] + ", "
                        f = 1
                        console.log(mName)
                    }
                })
                mName = mName.substring(0, mName.length - 2)
                if (f) {
                    $(".stock-error-message").text("Demand of " + mName + " is more than supply so capped to max stock available.")
                    $(".stock-error-message").attr("class", "text-danger stock-error-message")
                }
                raw = raw + "";
                $(".demandTable").hide()
                $(".demandTableTwo").show()
                $(".demandTableTwoBody").html(raw)
            }

        }
    })
}


$("#loginForm").on("submit", function (e) {
    e.preventDefault();
    var flag = 1;
    $(".opResult").text("");
    if ($("#UserName").val().length == 0) {
        flag = 0;
        $("[data-valmsg-for=UserName]").text("Username is required")
    }
    else {
        $("[data-valmsg-for=UserName]").text("")
    }

    if ($("#Password").val().length == 0) {
        flag = 0;
        $("[data-valmsg-for=Password]").text("Password is required")
    }
    else {
        $("[data-valmsg-for=Password]").text("")
    }
    if (flag == 1) {
        var form = $(this).serialize();
        $.ajax({
            type: "POST",
            url: "User/Login",
            data: form,
            success: function (data) {
                console.log(data)
                if (data == "Invalid username/password") {
                    $(".opResult").text("Either Username or Password is Invalid");
                    $(".opResult").attr("class", "text-danger");
                }
                else if (data == "Server didn't respond") {
                    $(".opResult").text(data);
                    $(".opResult").attr("class", "text-danger");
                }
                else window.location.href = "User/Index";
            }
        })
    }

})