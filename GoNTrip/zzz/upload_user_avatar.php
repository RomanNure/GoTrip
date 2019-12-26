<?php
	header("Access-Control-Allow-Origin: *");
	
	foreach($_POST as $k => $v) {
		file_put_contents("user_log.txt", $k." => ".$v."\n", FILE_APPEND);
	}
	foreach($_FILES as $k => $v) {
		foreach($v as $f => $i) {
			file_put_contents("user_log.txt", $k." => ".$f." => ".$i."\n", FILE_APPEND);
		}
	}

	$user_dir = 'imgs/users/'.$_POST['userId'];
	file_put_contents("user_log.txt", 'user '.$_POST['userId']." wants to update his avatar\n", FILE_APPEND);
	
	if(!file_exists($user_dir)) {
		mkdir($user_dir);
		file_put_contents("user_log.txt", $user_dir." created\n", FILE_APPEND);
	}
	
	$file = $_FILES['avatar'];
	$ufn = $user_dir."/".basename($file["name"]);
	move_uploaded_file($file['tmp_name'], $ufn);
	
	file_put_contents("user_log.txt", "avatar is in ".$ufn."\n", FILE_APPEND);
	
	$response = '{ "path" : "'.$ufn.'"}';
	file_put_contents("user_log.txt", $response." was responsed\n", FILE_APPEND);
	
	echo $response;
?>