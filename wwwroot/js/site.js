// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let apiUrl = "https://forkify-api.herokuapp.com/api/v2/recipes";
let apiKey = "6e6825a7-9ec2-4bec-9e05-8798f24e5ba8";
async function GetRecipes(recipeName,id,isAllShow) {
    let resp = await fetch(`${apiUrl}?search=${recipeName}&key=${apiKey}`);
    let result = await resp.json();
    console.log(result);
    let Recipes = isAllShow ? result.data.recipes : result.data.recipes.slice(1, 7);
    showRecipes(Recipes, id);
}

function showRecipes(recipes, id) {
    $.ajax({
        contentType: "application/json;charset=utf-8",
        dataType: 'html',
        type: 'POST',
        url: '/Recipes/GetRecipecard',
        data: JSON.stringify(recipes),
        success: function (htmlResult) {
            $('#' + id).html(htmlResult);
            getAddedCarts();
        }
    });
}

async function getOrderRecipe(id,showId) {

    let resp = await fetch(`${apiUrl}/${id}?key=${apiKey}`);
    let result = await resp.json();
    console.log(result);
    let recipe = result.data.recipe;
    showOrderRecipeDetails(recipe, showId)
}

function showOrderRecipeDetails(details, showId) {
    $.ajax({
        url: '/Recipes/ShowOrder',
        data: details,
        dataType: 'html',
        type: 'POST',
        success: function (htmlresult) {
            $('#' + showId).html(htmlresult);
        }
    });
}

function quantity(option) {
    let qty = $('#qty').val();
    let price = parseInt($('#price').val());
    let totalAmount = 0;
    if (option === 'inc') {
        qty = parseInt(qty) + 1;
       
    }
    else {
        qty = qty == 1 ? qty : qty - 1;

    }
    totalAmount = price * qty;
    $('#qty').val(qty);
    $('#totalamount').val(totalAmount);
}


//Add To Cart

async function cart() {
    let itag = $(this).children('i')[0];
    let recipeId = $(this).attr('data-recipeId')
     //console.log(recipeId);
     //console.log(itag)
    if ($(itag).hasClass('fa-regular')) {
        let resp = await fetch(`${apiUrl}/${recipeId}?key=${apiKey}`);
        let result = await resp.json();
        let cart = result.data.recipe;
        cart.RecipeId = recipeId;
        delete cart.id;
      /*  console.log(cart);*/
        //console.log(result);
        cartRequest(cart, 'SaveCart', 'fa-solid', 'fa-regular', itag,false)
    } else {
        let data = { Id: recipeId }
        cartRequest(data, 'RemoveCart', 'fa-regular', 'fa-solid', itag, false)
    }
}

function cartRequest(data, action, addcls, removecls, iTag,isReload) {
    $.ajax({
        url: '/Cart/' + action,
        type: 'POST',
        data: data,
        success: function (response) {
            /*console.log(response)*/;
            if (isReload) {
                location.reload();
            }
            else {
                $(iTag).addClass(addcls);
                $(iTag).removeClass(removecls);
            }
            
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function getAddedCarts() {
    $.ajax({
        url: '/Cart/GetAddedCarts/',
        type: 'GET',
        dataType: 'json',
        success: function (result) {

            //console.log(result)
            $('.addToCartIcon').each((index, spanTag) => {
                /*console.log(itag);*/
                let recipeId = $(spanTag).attr("data-recipeId");
                for (var i = 0; i < result.length; i++) {
                    if (recipeId == result[i]) {
                        let iTag = $(spanTag).children('i')[0];
                        $(iTag).addClass('fa-solid');
                        $(iTag).removeClass('fa-regular');
                        break;
                    }
                }
            })
        },
        error: function (err) {
            console.log(err)
        }
    });
}


function getCartItem() {
    $.ajax({
        url: '/Cart/GetCartItems',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            $('#showCartList').html(result)
           // console.log(result);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function removeCartFromList(id) {
    // console.log("hii its clicked")
    let data = { Id: id };
    cartRequest(data, 'RemoveCart', null, null, null, true);
}

//console.log($(this))
//let itag = $(this).children('i')[0];
``
//let recipeId = $(this).attr('data-recipeId');
//console.log(itag);
//console.log(recipeId);
//if ($(itag).hasclass('fa-regular')) {
//    let resp = await fetch(`${apiurl}/${recipeid}?key=${apikey}`);
//    let result = await resp.json();
//    console.log(result);
//}
//else {

//}