$(function() {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.chatHub;
    // Объявление функции, которую хаб вызывает при получении сообщений
    chat.client.addChatMessage = function(message) {
        if (message.includes($("#seller").val())) {
            $('#chatroom').append('<p class="text-right"><b>' + htmlEncode(message));
        } else {
            chat.server.sendMessage(message);
        }
    };
    var i = 0;
    chat.client.addMessage = function(message, urls) {
        urls.forEach(function(element) {
            $('#chatroom').append('<img id="img' + i + '" width="20" height="20" style="margin-right:3px">');
            previewFile(i++, element);
        });
        $('#chatroom').append('<p class="text-left"><b>' + htmlEncode(message));
    }

    // Открываем соединение
    $.connection.hub.start().done(function() {
        $('#sendmessage').click(function() {
            chat.server.sendChatMessage($('#toUserID').val(), $("#seller").val(), $('#message').val());
            $('#message').val('');
        });
    });
});

// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

function previewFile(id, url) {
    // FileReader instance
    url = "Image\\" + url.substring(url.indexOf("Image"));
    var element = $('#img' + id);
    element.attr("src", url);
}