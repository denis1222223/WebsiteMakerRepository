//var contentJSON = $.parseJSON(jsonContentString);
fillContent();

//var menuJSON = $.parseJSON(jsonMenuString);
fillMenu();

function fillMenu() {
    if (menuJSON.horizontal_menu_exist) {
        addMenuItems(menuJSON.horizontal_menu, '#horizontalMenu');
    }
    if (menuJSON.vertical_menu_exist) {
        addMenuItems(menuJSON.vertical_menu, '#verticalMenu');
    }
}

function addMenuItems(items, menu) {
    $.each(items, function (i, item) {
        $(menu).find('.add').before("<li><a href='" + item.link + "' class='menu-item' >" + item.title + "</a></li>");
    })
}

function fillContent() {
    $.each(contentJSON.content, function (i, item) {
        addContentItem(item);
    })
}

function addContentItem(item) {
    switch (item.content_type) {
        case "picture": { addPicture(item); break; }
        case "text": { addText(item); break; }
        case "video": { addVideo(item); break; }
    }
}

function addPicture(item) {
    var newPicture = generatePictureItem(item.value);
    $('#' + item.place).append(newPicture);
}

function addVideo(item) {
    var newVideo = generateVideoItem(item.value);
    $('#' + item.place).append(newVideo);
}

function addText(item) {
    var newText = generateTextItem(item.value);
    $('#' + item.place).append(newText);
}

function generatePictureItem(src){
    var picMarkup = "<li class='picture item sortable list-group-item col-md-12 col-lg-12 col-sm-12 col-xs-12'"+
        "style='position: relative; left: 0px; top: 0px; border: none; background-color: transparent;'>"+
        "<img src='" + src + "'></li>"
    return picMarkup;
}

function generateVideoItem(src) {
    var video = $("<video id='video' width='100%' height='320px'><source src='" + src + "'type='video/youtube' ></video>");
    var videoDOM = $("<li class='video item sortable list-group-item' style='position: relative; left: 0px; top: 0px;'></li>");
    videoDOM.html(video.prop('outerHTML'));
    videoDOM.children('#video').mediaelementplayer();
    return videoDOM;
}

function generateTextItem(text) {
    var textMarkup = "<li class='picture item sortable list-group-item col-md-12 col-lg-12 col-sm-12 col-xs-12'" +
        "style='position: relative; left: 0px; top: 0px; border: none; background-color: transparent;'>" +
        "<div><p>" + text + "</p></div></li>"
    return textMarkup;
}