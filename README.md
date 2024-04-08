# Tweener
UniTaskで動作するTweener

# Install
PackageManager->+ Add Package from git URLで以下のURLを指定する<br>
```
https://github.com/hadoumune/Tweener.git?path=Packages/Tweener
```

# 使い方
```
    async UniTask Start()
    {
        Tweener.DefaultEasing = EaseType.BounceOut;

        var mesh = GetComponentInChildren<MeshRenderer>();

        while(true){
            await gameObject.LocalRotYTo(200.0f,2.0f);
            await mesh.ColorTo( Color.red,1.0f);
            await gameObject.XTo(3.0f,2.0f);
            mesh.ColorTo( Color.green,1.0f).Forget();
            await gameObject.YOffset(3.0f,1.0f);
            await gameObject.XTo(-3.0f,1.0f);
            mesh.ColorTo( Color.white,1.0f).Forget();
            await gameObject.YOffset<Default>(-3.0f,1.0f);
        }
    }
```
CancelationTokenを省略した場合、元のgameObjectのGetCancellationTokenOnDestroyを利用する
RotationがQuaternionを使っていないためX軸回転でジンバルロックを起こしてしまう
