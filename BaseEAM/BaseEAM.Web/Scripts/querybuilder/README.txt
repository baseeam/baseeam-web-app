The file loading order is in Grunt config

var config = initConfig(grunt, {
        js_core_files: [
            'src/main.js',
            'src/defaults.js',
            'src/plugins.js',
            'src/core.js',
            'src/public.js',
            'src/data.js',
            'src/template.js',
            'src/utils.js',
            'src/model.js',
            'src/jquery.js'
        ],
        js_files_for_standalone: [
            'bower_components/jquery-extendext/jQuery.extendext.js',
            'bower_components/doT/doT.js',
            'dist/js/query-builder.js'
        ]
    });