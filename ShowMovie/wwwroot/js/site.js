

setInterval(InvokeHomeOndex, 10000);
setInterval(DeleteMovie, 30000);

function InvokeHomeOndex() {
    $.ajax({
        url: `/Home/Index`,
        method: "GET",

        success: function (data) {
            //console.log("s");
        }
    })
}

function DeleteMovie() {
    $.ajax({
        url: `/Home/DeleteMovie`,
        method: "GET",

        success: function (data) {
            //console.log("s");
        }
    })
}
