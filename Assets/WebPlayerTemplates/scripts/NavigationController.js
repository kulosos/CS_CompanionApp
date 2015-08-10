/*
 * Navigation Script
 *
 * @brief: Navigation, Listener, Events & content loading on entire site
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: April 2015
 *
 */
//-----------------------------------------------------------------------------

var headerHeight = 60;
var duration = 500;

var isGameUIActive = false;
var isMainMenuActive = false;
var isLoading = false;
var headerMenuListener = false;

function initNavigation() {

	// set header hidden to scroll from top
	$('.header').css("top", headerHeight * -1);

	// load start page default
	loadPageContent("start");
}

function loadPageContent(page, callback){	
	var filepath = "./content/" + page + ".html";
	$("#content").load(filepath, function(){
		setEventListeners(page);
		// CALLBACK
		if(callback != undefined && typeof callback == 'function') callback();
	});
}
// ----------------------------------------------
function switchGameUI(){
	//console.info("GameUI switched");
	
	if(isLoading){
		
	}
	
	if(!isGameUIActive){
		//loadGameUI();
		//showLoadingBar();
		isGameUIActive = true;
	}
	else{
		//unloadGameUI();
		//showLoadingBar();
		isGameUIActive = false;
	}
}
// ----------------------------------------------
function toggleMainMenu(){
	if(!isMainMenuActive){
		loadMainMenu();
		isMainMenuActive = true;
	}
	else{
		unloadMainMenu(false);		
		isMainMenuActive = false;
	}
}
// ----------------------------------------------
function showLoadingScreen(){
	loadPageContent("loadingbar");
	//unloadGameUI();	
}

function hideLoadingScreen() {
	console.info("hideLoadingBar");
	$("#content").find(".loadingBarWrapper").remove();
	//loadGameUI();
}
// ----------------------------------------------
function loadGameUI(){
	// translate out animation
	$('.header').animate({ "top": "0px" }, duration );
	$('#startpage .outerWrapper').animate({ "top": docHeight + "px"}, duration, function(){
		$("#startpage").remove();
	});
	$('.footerLogoFixConSim').animate({ "bottom": "-"+$(".footerLogoFixConSim").css("height") }, duration);
	$('.footer').animate({ "bottom": "-"+$(".footer").css("height") }, duration, function(){
		$('.footerLogoFixConSim').hide();
		$('.footer').hide();
	});
}

function unloadGameUI(){
	unloadMainMenu(false, function(){
		loadPageContent("start", function(){
			$('#startpage .outerWrapper').css("top", docHeight + "px");
			$('.header').animate({ "top": $(".header").css("height")+"px" }, duration, function(){
				// translate in animation
				$('.header').animate({ "top": "0px" }, duration);
				$('#startpage .outerWrapper').animate({ "top": "0px"}, duration);
				$('.footerLogoFixConSim').show();
				$('.footer').show();
				$('.footerLogoFixConSim').animate({ "bottom": "0px" }, duration);
				$('.footer').animate({ "bottom": "0px"}, duration);
			});
		});
	});
}

function loadMainMenu(callback){
	
	loadPageContent("menu", function(){
		
		$("#mainmenu").hide;
		$("#mainmenu").css("top", $(".header").css("height"));

		var headerHeight =  parseInt($(".header").css("height")) + 3;
		var owHeight = docHeight - headerHeight;
		var menuItemWidth = parseInt($("#mainmenu").css("width"))*-1;

		$("#mainmenu").css("height", owHeight+"px");
		$("#mainmenu .outerWrapper").css("height", owHeight+"px");
		$("#mainmenu").css("right", menuItemWidth);
		
		$("#mainmenu").show;
		$('#mainmenu').animate({ "right": "2px" }, duration, function() {
			// CALLBACK
			if(callback != undefined && typeof callback == 'function') callback();
		});
	});
}

function unloadMainMenu(setMenuInactive, callback){
	var menuItemWidth = parseInt($("#mainmenu").css("width"));
	$('#mainmenu').animate({ "right": (menuItemWidth * -1)+"px" }, duration, function(){
		$("#mainmenu").remove();
		
		// CALLBACK
		if(callback != undefined && typeof callback == 'function') callback();
	});
	
	if(setMenuInactive){
		isMainMenuActive = false;
	}
}

function setEventListeners(page){
	
	if(page == "start"){
		
		if(!headerMenuListener){
			mainMenuBtn = document.getElementById("headerMenuBtn");
			if(mainMenuBtn != null){
				mainMenuBtn.addEventListener("click", function() {
					toggleMainMenu();
				});
			}
			headerMenuListener = true;
		}
		
		$( "#connectionInput" ).submit(function(event) {
		  connect();
		  //if(D)switchGameUI();
		  event.preventDefault();
		});
		return;
	}
	
	if(page == "menu"){
		bindMenuButtons();
		return;
	}
}

function bindMenuButtons(){
	$("body").find("#menuIdBackToStart").bind( "click", function() {
		unloadMainMenu(true, function(){
			unloadGameUI();
		});
	});
	
	$("body").find("#menuIdMap").bind( "click", function() {
		unloadMainMenu(true, function(){
			engine.trigger("loadScene", "01_Map");
		});
	});
	
	$("body").find("#menuIdRemoteControlDriving").bind( "click", function() {
		unloadMainMenu(true, function(){
			engine.trigger("loadScene", "02_RemoteControl_Driving");
		});
	});
	
	$("body").find("#menuIdRemoteControlCrane").bind( "click", function() {
		unloadMainMenu(true, function(){
			engine.trigger("loadScene", "03_RemoteControl_Crane");
		});
	});
	
}
