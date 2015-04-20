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
		// translate out animation
		$('.header').animate({ "top": "0px" }, duration );
		$('#startpage .outerWrapper').animate({ "top":  $(document).height()+ "px"}, duration );
		$('.footerLogoFixConSim').animate({ "bottom": "-"+$(".footerLogoFixConSim").css("height") }, duration);
		$('.footerLogoFixWB').animate({ "bottom": "-"+$(".footer").css("height") }, duration);
		$('.footer').animate({ "bottom": "-"+$(".footer").css("height") }, duration);
		isGameUIActive = true;
	}
	else{

		unloadMainMenu();
		$('.header').animate({ "top": "-60px" }, duration, function(){
			$("#content").load("./content/start.html", loadStartPage(), function(){

				$('#startpage .outerWrapper').css("top", $(document).height()+ "px");
				// translate in animation
				$('.header').animate({ "top": "0px" }, duration);
				$('#startpage .outerWrapper').animate({ "top": "0px"}, duration);
				$('.footerLogoFixConSim').animate({ "bottom": "0px" }, duration);
				$('.footerLogoFixWB').animate({ "bottom": "0px" }, duration);
				$('.footer').animate({ "bottom": "0px"}, duration);
			});
		});

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

function loadMainMenu(){
	$("#content").load("./content/menu.html", function(){

			backToStartBtn = document.getElementById("menuIdBackToStart");
	    	if(backToStartBtn != null){
				backToStartBtn.addEventListener("click", function() {
					switchGameUI();
				});
			}

			var headerHeight =  parseInt($(".header").css("height")) * 3;
			var docHeight = parseInt($(document).height());
			var owHeight = docHeight - headerHeight ;
			var menuItemWidth = parseInt($("#mainmenu").css("width"))*-1;
			
			$("#mainmenu").css("height", owHeight+"px");
			$("#mainmenu .outerWrapper").css("height", owHeight+"px");
			$("#mainmenu").css("right", menuItemWidth);
			$('#mainmenu').animate({ "right": "2px" }, duration);
		});
}

function unloadMainMenu(){
	var menuItemWidth = parseInt($("#mainmenu").css("width"));
		
		$('#mainmenu').animate({ "right": "-"+menuItemWidth+"px" }, duration, function(){
			$("#mainmenu").remove();
		});
}





 
