$('#commentSend').click(function (e) {
    var commentText = $('#commentTextarea').val();
    if (commentText != "") {
        sendComment(commentText);
    }
    e.preventDefault();
});

var userName = $(location).attr('href').split("/")[3];
var siteUrl = $(location).attr('href').split("/")[4];

function sendComment(commentText) {
    $.ajax({
        type: 'POST',
        dataType: 'text',
        url: "/comment",
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
    $('#commentList').append(commentMarkup);
    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
    $('#commentTextarea').val("");
}

function rateClick() {
    $.ajax({
        type: 'POST',
        dataType: 'text',
        url: "/rate",
        data: {
            'userName': userName,
            'siteUrl': siteUrl
        },
        success: function (data) {
            setRating(data);
        }
    });
}

function setRating(rating) {
    $('#ratingValue').text(rating);
    if ($('#ratingStar').hasClass('glyphicon-star')) {
        $('#ratingStar').replaceWith("<span id='ratingStar' class='glyphicon glyphicon-star-empty'></span>");
    } else {
        $('#ratingStar').replaceWith("<span id='ratingStar' class='glyphicon glyphicon-star'></span>");
    }
}