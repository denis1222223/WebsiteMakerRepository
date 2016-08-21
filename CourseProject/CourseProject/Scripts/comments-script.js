$('#commentSend').click(function (e) {
    var commentText = $('#commentTextarea').val();
    if (commentText != "") {
        sendComment(commentText);
    }
    e.preventDefault();
});

function sendComment(commentText) {
    var userName = $(location).attr('href').split("/")[3];
    var siteUrl = $(location).attr('href').split("/")[4];

    $.ajax({
        type: 'POST',
        dataType: 'text',
        url: "/add_comment",
        data: {
            'userName': userName,
            'siteUrl': siteUrl,
            'commentText': commentText,
        },
        success: function (data) {
            if (data != undefined) {
                appendComment($.parseJSON(data));
            }
        }
    })
}

var commentTemplate = "<li class='list-group-item comment'><div class='commentInfo row'><div class='col-lg-2 col-md-2 col-sm-2 col-xs-2'>" +"<img src='{author_picture}' class='img-circle' style='width:60px; height:60px;'></div><div class='author-date'>" +"<div class='commentAuthor'>{author_name}</div><div class='commentDate'>{date_time}</div></div></div><hr>" +"<div class='commentText'><p>{text}</p></div></li>"

function makeCommentMarkup(comment) {
    var commentMarkup = commentTemplate.replace("{author_picture}", comment["author_picture"])
        .replace("{author_name}", comment["author_name"])
    .replace("{date_time}", comment["date_time"])
    .replace("{text}", comment["text"]);
    return commentMarkup;
}

function appendComment(comment) {
    var commentMarkup = makeCommentMarkup(comment);  
    if ($('.comment').length == 0) {
        $('#commentList').before($(commentMarkup));
    } else {
        $('.comment').first().before($(commentMarkup));
    }
    $('#commentTextarea').val("");
}

$('#ratingBox').click(function (e) {
    var url;
    if ($(location).attr('href').split("/").length == 7)
        url = '../add_rating';
    else
        url = 'add_rating';
    $.ajax({
        type: 'POST',
        url: url,
        success: function (data) {
            if (data != undefined) {
                setRating(data);
            } else {
                alert("Оценивать могут только авторизированные пользователи.");
            }
        }
    });
});

function setRating(rating) {
    $('#ratingValue').text(rating);
    if ($('#ratingStar').hasClass('glyphicon-star')) {
        $('#ratingStar').replaceWith("<span id='ratingStar' class='glyphicon glyphicon-star-empty'></span>");
    } else {
        $('#ratingStar').replaceWith("<span id='ratingStar' class='glyphicon glyphicon-star'></span>");
    }
}