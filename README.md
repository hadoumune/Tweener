# Tweener
UniTaskで動作するTweener

# Install
PackageManager->+ Add Package from git URLで以下のURLを指定する<br>
```
https://github.com/hadoumune/Tweener.git?path=Packages/Tweener
```

# 使い方
```
  await gameObject.XTo( 100.0f, 1.0f, EaseType.Linear );
  await transform.XTo( 100.0f, 1.0f, EaseType.Linear );
  await spriterenderer.XOffset( 100.0f, 1.0f, EaseType.Linear );
```
CancelationTokenを省略した場合、元のgameObjectのGetCancellationTokenOnDestroyを利用する
