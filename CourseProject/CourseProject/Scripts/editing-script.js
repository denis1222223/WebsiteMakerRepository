$(function () {
    $(".save").click(function () {
        var id = document.getElementById("Id");
        var htmlCode = document.documentElement.innerHTML.toString();
        alert("/sites/save/" + id.value);
        $.ajax({
            type: "POST",
            url: "/sites/save/" + id.value,
            data: htmlCode,
            datatype: "text",
            success: function (data) {
                window.location.replace("/sites");
            }

        });
    });
});