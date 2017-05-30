var textEditor, definitionEditor;
$(function () {
    definitionEditor = editormd({
        id: "definition-editormd",
        toolbarIcons: "simple",
        width: "100%",
        height: 230,
        path: "/Scripts/Editor.md/lib/",
        pluginPath: "/Scripts/Editor.md/plugins/",
        emoji: false,
        imageUpload: true,
        placeholder: "Введите краткое описание статьи..."
    });
    textEditor = editormd({
        id: "text-editormd",
        toolbarIcons: "full",
        width: "100%",
        height: 500,
        path: "/Scripts/Editor.md/lib/",
        pluginPath: "/Scripts/Editor.md/plugins/",
        emoji: false,
        imageUpload: true,
        placeholder: "Введите основное содержание статьи..."
    });
});