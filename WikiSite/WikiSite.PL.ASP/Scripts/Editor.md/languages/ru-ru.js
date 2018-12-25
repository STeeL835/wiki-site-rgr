(function(){
    var factory = function (exports) {
        var lang = {
            name : "ru-ru",
            description : "Open source online Markdown editor.",
            tocTitle    : "Table of Contents",
            toolbar : {
                undo             : "Отменить(Ctrl+Z)",
                redo             : "Вернуть(Ctrl+Y)",
                bold             : "Полужирный",
                del              : "Зачеркнутый",
                italic           : "Наклонный",
                quote            : "Цитата",
                ucwords          : "Конвертировать первые буквы слов в заглавные",
                uppercase        : "Заменить выделенный текст заглавными буквами",
				lowercase        : "Заменить выделенный текст строчными буквами",
                h1               : "Заголовок 1",
				h2               : "Заголовок 2",
				h3               : "Заголовок 3",
				h4               : "Заголовок 4",
				h5               : "Заголовок 5",
				h6               : "Заголовок 6",
                "list-ul"        : "Ненумерованный список",
                "list-ol"        : "Нумерованный список",
                hr               : "Горизонтальная линия",
                link             : "Ссылка",
                "reference-link" : "Ссылка на ссылку",
                image            : "Изображение",
                code             : "Вставка кода",
                "preformatted-text" : "Неформатированный текст / Блок кода (с табуляцией)",
                "code-block"     : "Блок кода (Несколько языков)",
                table            : "Таблицы",
                datetime         : "Дата и время",
                emoji            : "Emoji",
                "html-entities"  : "HTML элементы",
                pagebreak        : "Разрыв страницы",
                watch            : "Отключить предпросмотр",
                unwatch          : "Включить предпросмотр",
                preview          : "HTML предпросмотр (Shift + ESC для выхода)",
                fullscreen       : "Полноэкранный режим (ESC для выхода)",
                clear            : "Очистить",
                search           : "Поиск",
                help             : "Помощь",
                info             : "О " + exports.title
            },
            buttons : {
                enter  : "Enter",
                cancel : "Cancel",
                close  : "Close"
            },
            dialog : {
                link : {
                    title    : "Ссылка",
                    url      : "Адрес",
                    urlTitle : "Название",
                    urlEmpty : "Ошибка: Пожалуйста, укажите адрес."
                },
                referenceLink : {
                    title    : "Ярлык на ссылку",
                    name     : "Название",
                    url      : "Адрес",
                    urlId    : "ID",
                    urlTitle : "Имя ссылки",
                    nameEmpty: "Ошибка: Имя ярлыка не может быть пустым.",
                    idEmpty  : "Ошибка: Пожалуйста, укажите ID ярлыка ссылки.",
                    urlEmpty : "Ошибка: Пожалуйста, укажите адрес ссылки."
                },
                image : {
                    title    : "Изображение",
                    url      : "Ссылка на изображение",
                    link     : "Ссылка-изображение",
                    alt      : "Текст при наведении",
                    uploadButton     : "Загрузить",
                    imageURLEmpty    : "Ошибка: Ссылка на изображение не может быть пустой.",
                    uploadFileEmpty  : "Ошибка: Невозможно загрузить пустой файл!",
                    formatNotAllowed : "Ошибка: Возможна только загрузка изображений определенного формата:"
                },
                preformattedText : {
                    title             : "Неформатированный текст / Код", 
                    emptyAlert        : "Ошибка: Поле не может быть пустым."
                },
                codeBlock : {
                    title             : "Блок кода",         
                    selectLabel       : "Языки: ",
                    selectDefaultText : "Выберите язык кода...",
                    otherLanguage     : "Другие языки",
                    unselectedLanguageAlert : "Ошибка: Выберите язык.",
                    codeEmptyAlert    : "Ошибка: Заполните поле для кода."
                },
                htmlEntities : {
                    title : "HTML элементы"
                },
                help : {
                    title : "Помощь"
                }
            }
        };
        
        exports.defaults.lang = lang;
    };
    
	// CommonJS/Node.js
	if (typeof require === "function" && typeof exports === "object" && typeof module === "object")
    { 
        module.exports = factory;
    }
	else if (typeof define === "function")  // AMD/CMD/Sea.js
    {
		if (define.amd) { // for Require.js

			define(["editormd"], function(editormd) {
                factory(editormd);
            });

		} else { // for Sea.js
			define(function(require) {
                var editormd = require("../editormd");
                factory(editormd);
            });
		}
	} 
	else
	{
        factory(window.editormd);
	}
    
})();