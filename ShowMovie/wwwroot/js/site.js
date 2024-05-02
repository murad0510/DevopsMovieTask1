

setInterval(InvokeHomeOndex, 20000);
function InvokeHomeOndex() {
    $.ajax({
        url: `/Home/Index`,
        method: "GET",

        success: function (data) {
            //console.log("s");
        }
    })
}

