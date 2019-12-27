<?php

	if(isset($_COOKIE["admin_auth"]) && $_COOKIE["admin_auth"] == "true" && isset($_POST['dir']) && isset($_FILES['file'])) {
		
		$file = $_FILES['file'];
		move_uploaded_file($file['tmp_name'], $_POST['dir']."/".basename($file["name"]));
		
		echo $_POST['dir']."/".$file['tmp_name'];
		
		if(isset($_POST['redirect']) && $_POST['redirect'] != "")
			Header("Location: ".$_POST['redirect']);
	}
?>