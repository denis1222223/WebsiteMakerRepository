
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
        if (itemJSON["value"] != undefined)
            items.push(itemJSON);
    });
    return items;
}

function makeItem(item, place) {
    var itemJSON = {};
    itemJSON["place"] = place;
    var itemType = item.classList[0];
    itemJSON["content_type"] = itemType;
    itemJSON["value"] = $(item).data("value");
    return itemJSON;
}

function getMenuJSON() {
    var jsonObject = {};
    jsonObject["vertical_menu_exist"] = menuJSON.vertical_menu_exist;
    jsonObject["horizontal_menu_exist"] = menuJSON.horizontal_menu_exist;
    jsonObject["vertical_menu"] = getMenuItemsJSON("vertical_menu");
    jsonObject["horizontal_menu"] = getMenuItemsJSON("horizontal_menu");
    return jsonObject;
}

function getMenuItemsJSON(menuId) {
    var items = [];
    $.each($('#' + menuId).find('.menu-item'), function (i, item) {
        var menuItem = {};
        menuItem["link"] = $(item).attr("href");
        menuItem["title"] = $(item).text();
        items.push(menuItem);
    });
    return items;
}