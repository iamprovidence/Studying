// string format
if (!String.prototype.format) 
{
    String.prototype.format = function() 
    {
        var args = arguments;

        return this.replace(/{(\d+)}/g, function(match, number) 
        { 
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

$(document).ready(function()
{
    my_skills_data();
    books_read_data();
    bind_thumbtack();

    $("#left-panel .fas").trigger("click");
});

function bind_thumbtack()
{
    // make panel fixed
    $("#left-panel .fas").click(function()
    {
        $(this).toggleClass("active");
        
        var fixed_block = $("#fixed-block");

        // fix
        if ($(this).hasClass("active"))
        {
            fixed_block.css({ position: 'fixed' });
            fixed_block.width($("#left-panel").width());

            $(window).resize(function() 
            {
                fixed_block.width($("#left-panel").width());
            });
        }
        else // unfix
        {
            fixed_block.removeAttr("style")
            $(window).unbind("resize");
        }
    });
}

// SKILLS
function my_skills_data()
{
    var data_array = 
    [
        {
            year: 2015,
            technologies: ['HTML', 'CSS', 'JavaScript', 'JQuery', 'Less', 'SEO']
        },
        {
            year: 2016,
            technologies: ['Photoshop', 'MySql', 'PHP', 'C++', 'OOP', 'Design patterns']
        },
        {
            year: 2017,
            technologies: ['C#', 'Windows Forms', 'Processing', 'Neural networks']
        },
        {
            year: 2018,
            technologies: ['WPF', 'ADO.Net', 'Entity Framework', 'Entity Framework Core']
        },
        {
            year: 2019,
            technologies: ['Java', 'ASP.Net Core', 'Bootstrap']
        },
    ];

    var timeline = $("#timeline");
    var template = $("#timeline-template-block").text();

    var isLeft = true;
    for (var i = 0; i < data_array.length; ++i)
    {
        var technologies = "";
        for (var j = 0; j < data_array[i].technologies.length; ++j)
        {
            technologies += `<li>${data_array[i].technologies[j]}</li>`;
        }

        // append block
        timeline.append(template.format(isLeft ? "left" : "right", data_array[i].year, technologies));
        isLeft =! isLeft;
    }
}

// BOOKS
function books_read_data()
{
    var data_array =
    [
        {
            css_class: 'pink',
            language: 'C++',
            book_name: 'Primer plus',
            author: 'Stephen Prata'
        },
        {
            css_class: 'yellow',
            language: 'JavaScript',
            book_name: 'The Definitive Guide',
            author: 'David Flanagan'
        },
        {
            css_class: 'blue',
            language: 'PHP',
            book_name: 'Learning PHP, MySQL, & JavaScript',
            author: 'Robin Nickson'
        },
        {
            css_class: 'green',
            language: 'C#',
            book_name: 'Professional C# 7 and .NET Core 2.0',
            author: 'Christian Nagel'
        },
        {
            css_class: 'red',
            language: 'ASP',
            book_name: 'Pro ASP.Net Core MVC 2',
            author: 'Adam Freeman'
        },
        
    ];

    var books_list = $("#books-list");
    var template = $("#books-template-block").text();

    // add data
    for (var i = 0; i < data_array.length; ++i)
    {
        books_list.append(template.format(data_array[i].css_class, data_array[i].language, data_array[i].book_name, data_array[i].author));
    }
}