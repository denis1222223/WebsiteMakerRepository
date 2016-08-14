$(function () {
    $(".save").click(function () {
        var htmlCode = document.documentElement.innerHTML.toString();
        alert(htmlCode);
        $.ajax({
            type: "POST",
            url: "/sites/save",
            data: htmlCode,
            datatype: "text",
        });
    });
});