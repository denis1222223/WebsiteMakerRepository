
function getContentJSON() {
    var jsonObject = {};
    jsonObject["content_template"] = contentJSON.content_template;
    jsonObject["content"] = getContentItemsJSON();
    return jsonObject;
}

function getContentItemsJSON() {
    var items = [];
    $.each($('.templateArea'), function (i, area) {
        var allAreaItems = getAllAreaItems(area);
        items = items.concat(allAreaItems);
    });
    return items;
}

function getAllAreaItems(area) {
    var items = [];
    $.each(area.children, function (i, item) {
        var itemJSON = makeItem(item, area.id);
        items.push(itemJSON);
    });
    return items;
}

function makeItem(item, place) {
    var itemJSON = {};
    itemJSON["place"] = place;
    var itemType = item.classList[0];
    itemJSON["content_type"] = itemType;
    itemJSON["value"] = getItemValue(item, itemType);
    return itemJSON;
}

function getItemValue(item, itemType) {
    switch (itemType) {
        case "video": { return getVideoValue(item); break; }
        case "picture": { return getPictureValue(item); break; }
        case "text": { return getTextValue(item); break; }
    }
}

function getVideoValue(item) {
    return $(item).find('#video').find('source').prop("src");
}

function getTextValue(item) {
    return $(item).find('div').find('p').text();
}

function getPictureValue(item) {
    return $(item).find('img').prop("src");
}

function getMenuJSON() {
    var jsonObject = {};
    jsonObject["vertical_menu_exist"] = menuJSON.vertical_menu_exist;
    jsonObject["horizontal_menu_exist"] = menuJSON.horizontal_menu_exist;
    jsonObject["vertical_menu"] = getMenuItemsJSON("verticalMenu");
    jsonObject["horizontal_menu"] = getMenuItemsJSON("horizontalMenu");
    return jsonObject;
}

function getMenuItemsJSON(menuId) {
    var items = [];
    $.each($('#' + menuId).find('.menu-item'), function (i, item) {
        var menuItem = {};
        menuItem["link"] = $(item).prop("href");
        menuItem["title"] = $(item).text();
        items.push(menuItem);
    });
    return items;
}