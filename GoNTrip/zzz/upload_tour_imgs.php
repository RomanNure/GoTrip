<?php
	header("Access-Control-Allow-Origin: *");
	
	foreach($_POST as $k => $v) {
		file_put_contents("tour_log.txt", $k." => ".$v."\n", FILE_APPEND);
	}
	foreach($_FILES as $k => $v) {
		foreach($v as $f => $i) {
			file_put_contents("tour_log.txt", $k." => ".$f." => ".$i."\n", FILE_APPEND);
		}
	}
	
	/*$file = $_FILES['avatar'];
	file_put_contents("company_log.txt", $file["name"], $file['tmp_name']);*/
	
	/*$company_dir = 'imgs/companies/'.$_POST['companyId'];
	file_put_contents("log.txt", 'user '.$_POST['userId'].' wants to update his avatar', "w");
	
	if(!file_exists($user_dir)) {
		mkdir($user_dir);
		file_put_contents("log.txt", $user_dir." created", "w");
	}
	
	$ufn = $user_dir."/".basename($file["name"]);
	move_uploaded_file($file['tmp_name'], $ufn);
	
	file_put_contents("log.txt", "avatar is in ".$ufn, "w");
	
	$response = '{ "path" : "'.$ufn.'"}';
	file_put_contents("log.txt", $response." was responsed", "w");
	
	echo $response;*/
?>