/*
 * Navigation Script
 *
 * @brief: Navigation & content loading on entire site
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: April 2015
 *
 */
//-----------------------------------------------------------------------------

var D = true; // TOGGLE DEBUG CONSOLE OUTPUTS

var headerHeight = 60;
var joinGame1;
var duration = 500;

var isGameUIActive = false;
var isMainMenuActive = false;

$(document).ready(function() {

	// set header hidden to scroll from top
	$('.header').css("top", headerHeight * -1);

	$("#content").load("./content/start.html", loadStartPage());
});

function loadStartPage(){

	
	//blurElement(".start_contentBox", ".outerWrapper" );
	//$('body').css("background", "red");

    setTimeout(function () { 

    	mainMenuBtn = document.getElementById("headerMenuBtn");
    	if(mainMenuBtn != null){
			mainMenuBtn.addEventListener("click", function() {
				toggleMainMenu();
			});
		}

		joinGame1 = document.getElementById("game1");
		if(joinGame1 != null){
			joinGame1.addEventListener("click", function() {
				switchGameUI();

			});
		}

		/*    	
    	$('.start_hlLogo').css("background", "red");
		//$("#content").find('.start_hlLogo').css("background", "red");
		
		$('.start_contentBox').blurjs({
		    source: 'body',
		    radius: 10,
		    overlay: 'rgba(255,255,255,0.5)'
    	});
		*/
   	}, 500);
}

function switchGameUI(){
	if(!isGameUIActive){
		loadGameUI()
		isGameUIActive = true;
	}
	else{
		unloadGameUI();
		isGameUIActive = false;
	}
}

function toggleMainMenu(){
	if(!isMainMenuActive){
		loadMainMenu();
		isMainMenuActive = true;
	}
	else{
		unloadMainMenu();		
		isMainMenuActive = false;
	}
}

function loadGameUI(){
	// translate out animation
	$('.header').animate({ "top": "0px" }, duration );
	$('#startpage .outerWrapper').animate({ "top": globals.docHeight + "px"}, duration, function(){
		$("#startpage").remove();
	});
	$('.footerLogoFixConSim').animate({ "bottom": "-"+$(".footerLogoFixConSim").css("height") }, duration);
	$('.footer').animate({ "bottom": "-"+$(".footer").css("height") }, duration);
}

function unloadGameUI(){
	unloadMainMenu(function(){
		$("#content").load("./content/start.html", loadStartPage(), function(){
			$('#startpage .outerWrapper').css("top", globals.docHeight + "px");
			
			$('.header').animate({ "top": $(".header").css("height")+"px" }, duration, function(){
				// translate in animation
				$('.header').animate({ "top": "0px" }, duration);
				$('#startpage .outerWrapper').animate({ "top": "0px"}, duration);
				$('.footerLogoFixConSim').animate({ "bottom": "0px" }, duration);
				$('.footer').animate({ "bottom": "0px"}, duration);
			});
		});
	});
}


function loadMainMenu(callback){

	$("#content").load("./content/menu.html", function(){

		backToStartBtn = document.getElementById("menuIdBackToStart");
    	if(backToStartBtn != null){
			backToStartBtn.addEventListener("click", function() {
				switchGameUI();
			});
		}

		$("#mainmenu").css("top", $(".header").css("height"));

		var headerHeight =  parseInt($(".header").css("height")) + 3;
		var owHeight = globals.docHeight - headerHeight ;
		var menuItemWidth = parseInt($("#mainmenu").css("width"))*-1;
		
		$("#mainmenu").css("height", owHeight+"px");
		$("#mainmenu .outerWrapper").css("height", owHeight+"px");
		$("#mainmenu").css("right", menuItemWidth);
		$('#mainmenu').animate({ "right": "2px" }, duration, function() {
			// CALLBACK
			if(callback != undefined && typeof callback == 'function') callback();
		});
	});

   
}

function unloadMainMenu(callback){
	var menuItemWidth = parseInt($("#mainmenu").css("width"));
		
	$('#mainmenu').animate({ "right": (menuItemWidth * -1)+"px" }, duration, function(){
		$("#mainmenu").remove();
		// CALLBACK
		if(callback != undefined && typeof callback == 'function') callback();
	});

   
}





 
