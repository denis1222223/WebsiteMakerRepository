$(function () {
    $(".save").click(function () {
        var contentJson = getContentJSON();
        var menuJson = getMenuJSON();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '',
            data: {
                'contentJson': contentJson,
                'menuJson': menuJson
            }
        });
    });
});

function SavePage() {
    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '',
        data: { 'contentJson': null, 'menuJson': null },
    });
}

$(function () {
    $(".menu-item").click(function () {
        var link = $(this).attr('href');
        event.preventDefault();
        SavePage();
        $(location).attr('href', '/' + link + '/edit');
    });
});

$(function () {
    $(".add").click(function () {
        event.preventDefault();
        openModal('page');
    });
});

function submitForm() {
    var pageForm = $('#createForm');
    $.validator.unobtrusive.parse(pageForm);
    pageForm.validate();
    var newPage = $('#Url').val();
    pageForm.attr('action', '../' + newPage + '/create');
    pageForm.submit();
    return newPage;
}

function createPage() {
    SavePage();
    var newPage = submitForm();
    $(this).attr('data-dismiss', "modal");
}

//////////////////////////

$('#toolbar').draggable({
    containment: "html",
    axis: "y"
});

var toolMarkupPicture = $("<li class='picture item sortable list-group-item'><img src='http://res.cloudinary.com/website-maker/image/upload/v1471180013/toolbar/picture.png'></li>")
var toolMarkupVideo = $("<li class='video item sortable list-group-item'><img src='http://res.cloudinary.com/website-maker/image/upload/v1471180013/toolbar/video.png'></li>")
var toolMarkupText = $("<li class='text item sortable list-group-item'><img src='http://res.cloudinary.com/website-maker/image/upload/v1471180013/toolbar/text.png'></li>")

function initializeTool(toolType, markup) {
    $(toolType).draggable( {
        connectToSortable: '.templateArea',
        containment: "html",
        helper: function () {
            return markup.clone(true);
        },
        start: function () {
            $('.templateArea').addClass("dragging-active");
        },
        stop: function () {
            $(".item").unbind("click");
            $(".item").bind("click", clickHandler);
            $('.templateArea').removeClass("dragging-active");
        }
    });
}

initializeTool('#toolPictrue', toolMarkupPicture);
initializeTool('#toolVideo', toolMarkupVideo);
initializeTool('#toolText', toolMarkupText);

$('.templateArea').sortable({
    connectWith: '.templateArea',
    stop: function () {
        $('.templateArea').removeClass("dragging-active");
    },
    start: function () {
        $('.templateArea').addClass("dragging-active");
    }
});

function clickHandler(e) {
    var itemType = e.currentTarget.classList[0];
    $('#active').removeAttr("id");
    $(e.currentTarget).attr("id", "active");
    openModal(itemType);
};

function openModal(modalType) {
    $.ajax({
        type: "GET",
        url: "/modal/" + modalType,
        success: function (data) {
            $(".modal-content").html(data);
            fillModal(modalType);
            $('#myModal').modal('show');
        }
    }); 
}

function fillModal(modalType) {
    switch (modalType) {
        case 'text': { fillText(); break; }
    }  
}

function fillText() {
    $('#modalTextarea').val($("#active").text());
}

function buttonDelete() {
    $('#active').remove();
};

function buttonOK() {
    $('#active').addClass("col-md-12 col-lg-12 col-sm-12 col-xs-12");
    $('#active').width("").height("");
    $('#active').css("border", "none");
    $('#active').css("background-color", "transparent");
    var itemType = $('#active')[0].classList[0];
    switch (itemType) {
        case 'text': { setText(); break; }
        case 'video': { setVideo(); break; }
        case 'picture': { setPicture(); break; }
    }
};

function setText() {
    var newText = $('#modalTextarea').val();
    $('#active').html("<div><p>" + newText + "</p></div>");
}

function setPicture() {
    $('.export').click();
}

function setVideo() {
    var video = $("<video id='video' width='100%' height='320px'><source src='" + $('#modalVideo').val() + "'type='video/youtube' ></video>");
    $('#active').html(video.prop('outerHTML'));
    $('#active').children('#video').mediaelementplayer();
}