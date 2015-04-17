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

	loadStartPage();

});

function loadStartPage(){

	console.log("DOC READY - navigation class - loadStartPage");

	$("#content").load("./content/start.html", onPageLoadingFinished());
	//blurElement(".start_contentBox", ".outerWrapper" );
	//$('body').css("background", "red");
}


function onPageLoadingFinished(){

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
		$('.outerWrapper').animate({ "top":  $(document).height()+ "px"}, duration );
		$('.footerLogoFixConSim').animate({ "bottom": "-"+$(".footerLogoFixConSim").css("height") }, duration);
		$('.footerLogoFixWB').animate({ "bottom": "-"+$(".footer").css("height") }, duration);
		$('.footer').animate({ "bottom": "-"+$(".footer").css("height") }, duration);
		isGameUIActive = true;
	}
	else{
		// translate in animation
		$('.header').animate({ "top": "0px" }, duration);
		$('.outerWrapper').animate({ "top": "0px"}, duration);
		$('.footerLogoFixConSim').animate({ "bottom": "0px" }, duration);
		$('.footerLogoFixWB').animate({ "bottom": "0px" }, duration);
		$('.footer').animate({ "bottom": "0px"}, duration);
		isGameUIActive = false;
	}
}

function toggleMainMenu(){

	if(!isMainMenuActive){
		console.log("Main Menu in header clicked - toggleOn");
		$("#content").load("./content/menu.html", function(){
			console.log("Main Menu in header clicked - toggleOn - AFTER");
		});
		isMainMenuActive = true;
	}
	else{
		console.log("Main Menu in header clicked - toggleOff");
		isMainMenuActive = false;
	}
}





 
