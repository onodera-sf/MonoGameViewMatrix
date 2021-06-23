using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ViewMatrix
{
	/// <summary>
	/// ゲームメインクラス
	/// </summary>
	public class Game1 : Game
	{
    /// <summary>
    /// グラフィックデバイス管理クラス
    /// </summary>
    private readonly GraphicsDeviceManager _graphics = null;

    /// <summary>
    /// スプライトのバッチ化クラス
    /// </summary>
    private SpriteBatch _spriteBatch = null;

    /// <summary>
    /// スプライトでテキストを描画するためのフォント
    /// </summary>
    private SpriteFont _font = null;

    /// <summary>
    /// ボックスモデル
    /// </summary>
    private Model _box = null;

    /// <summary>
    /// XYZモデル
    /// </summary>
    private Model _xyz = null;

    /// <summary>
    /// カメラの位置
    /// </summary>
    private Vector3 _cameraPosition = new Vector3(2.0f, 4.0f, 20.0f);

    /// <summary>
    /// カメラの注視点
    /// </summary>
    private Vector3 _cameraTarget = new Vector3(0.0f, 0.0f, 0.0f);

    /// <summary>
    /// カメラの上方方向回転角度(radian)
    /// </summary>
    private float _cameraUpVectorRotate = 0.0f;

    /// <summary>
    /// 自動動作フラグ
    /// </summary>
    private int _autoMode = 0;

    /// <summary>
    /// マウスボタン押下フラグ
    /// </summary>
    private bool _isMousePressed = false;


    /// <summary>
    /// GameMain コンストラクタ
    /// </summary>
    public Game1()
    {
      // グラフィックデバイス管理クラスの作成
      _graphics = new GraphicsDeviceManager(this);

      // ゲームコンテンツのルートディレクトリを設定
      Content.RootDirectory = "Content";

      // マウスカーソルを表示
      IsMouseVisible = true;
    }

    /// <summary>
    /// ゲームが始まる前の初期化処理を行うメソッド
    /// グラフィック以外のデータの読み込み、コンポーネントの初期化を行う
    /// </summary>
    protected override void Initialize()
    {
      // TODO: ここに初期化ロジックを書いてください

      // コンポーネントの初期化などを行います
      base.Initialize();
    }

    /// <summary>
    /// ゲームが始まるときに一回だけ呼ばれ
    /// すべてのゲームコンテンツを読み込みます
    /// </summary>
    protected override void LoadContent()
    {
      // テクスチャーを描画するためのスプライトバッチクラスを作成します
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      // フォントをコンテンツパイプラインから読み込む
      _font = Content.Load<SpriteFont>("Font");

      // ボックスモデルを読み込み
      _box = Content.Load<Model>("Box");

      // XYZ軸モデルを読み込み
      _xyz = Content.Load<Model>("XYZ");
    }

    /// <summary>
    /// ゲームが終了するときに一回だけ呼ばれ
    /// すべてのゲームコンテンツをアンロードします
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: ContentManager で管理されていないコンテンツを
      //       ここでアンロードしてください
    }

    /// <summary>
    /// 描画以外のデータ更新等の処理を行うメソッド
    /// 主に入力処理、衝突判定などの物理計算、オーディオの再生など
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Update(GameTime gameTime)
    {
      KeyboardState keyState = Keyboard.GetState();
      MouseState mouseState = Mouse.GetState();
      GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

      // ゲームパッドの Back ボタン、またはキーボードの Esc キーを押したときにゲームを終了させます
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        Exit();
      }

      // マウスによる自動動作切り替え
      if (_isMousePressed == false &&
          mouseState.LeftButton == ButtonState.Pressed)
      {
        _isMousePressed = true;

        _autoMode = (_autoMode + 1) % 6;
      }
      _isMousePressed = mouseState.LeftButton == ButtonState.Pressed;

      float speed = 0.25f;

      // カメラの位置を移動させる
      _cameraPosition.X += gamePadState.ThumbSticks.Left.X * speed;
      _cameraPosition.Y += gamePadState.ThumbSticks.Left.Y * speed;
      if (keyState.IsKeyDown(Keys.Left))
      {
        _cameraPosition.X -= speed;
      }
      if (keyState.IsKeyDown(Keys.Right))
      {
        _cameraPosition.X += speed;
      }
      if (keyState.IsKeyDown(Keys.Down))
      {
        _cameraPosition.Y -= speed;
      }
      if (keyState.IsKeyDown(Keys.Up))
      {
        _cameraPosition.Y += speed;
      }
      if (_autoMode == 1)
      {
        _cameraPosition.X = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 10.0f;
      }
      if (_autoMode == 2)
      {
        _cameraPosition.Y = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 10.0f;
      }

      // カメラの注視点を移動させる
      _cameraTarget.X += gamePadState.ThumbSticks.Right.X * speed;
      _cameraTarget.Y += gamePadState.ThumbSticks.Right.Y * speed;
      if (keyState.IsKeyDown(Keys.A))
      {
        _cameraTarget.X -= speed;
      }
      if (keyState.IsKeyDown(Keys.S))
      {
        _cameraTarget.X += speed;
      }
      if (keyState.IsKeyDown(Keys.Z))
      {
        _cameraTarget.Y -= speed;
      }
      if (keyState.IsKeyDown(Keys.W))
      {
        _cameraTarget.Y += speed;
      }
      if (_autoMode == 3)
      {
        _cameraTarget.X = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 10.0f;
      }
      if (_autoMode == 4)
      {
        _cameraTarget.Y = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 10.0f;
      }

      // カメラの上方方向を回転させる
      _cameraUpVectorRotate -= gamePadState.Triggers.Left * speed * 0.2f;
      _cameraUpVectorRotate += gamePadState.Triggers.Right * speed * 0.2f;
      if (keyState.IsKeyDown(Keys.X))
      {
        _cameraUpVectorRotate -= speed * 0.2f;
      }
      if (keyState.IsKeyDown(Keys.C))
      {
        _cameraUpVectorRotate += speed * 0.2f;
      }
      if (_autoMode == 5)
      {
        _cameraUpVectorRotate = (float)gameTime.TotalGameTime.TotalSeconds;
      }

      // 登録された GameComponent を更新する
      base.Update(gameTime);
    }

    /// <summary>
    /// 描画処理を行うメソッド
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Draw(GameTime gameTime)
    {
      // 画面を指定した色でクリアします
      GraphicsDevice.Clear(Color.CornflowerBlue);

      // 深度バッファを有効にする
      GraphicsDevice.DepthStencilState = DepthStencilState.Default;

      // ビューマトリックス作成
      Matrix view = Matrix.CreateLookAt(
          _cameraPosition,
          _cameraTarget,
          Vector3.Transform(Vector3.Up,
                            Matrix.CreateRotationZ(_cameraUpVectorRotate))
      );

      // プロジェクションマトリックス作成
      Matrix projection = Matrix.CreatePerspectiveFieldOfView(
              MathHelper.PiOver4,
              (float)GraphicsDevice.Viewport.Width /
                  (float)GraphicsDevice.Viewport.Height,
              1.0f,
              100.0f
          );

      ///// ボックスモデル /////

      foreach (ModelMesh mesh in _box.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          // ライトの設定
          effect.EnableDefaultLighting();

          // ボックスの位置をカメラの注視点に合わせる
          effect.World = Matrix.CreateTranslation(_cameraTarget);

          // ビューマトリックスを設定
          effect.View = view;

          // プロジェクションマトリックス設定
          effect.Projection = projection;
        }

        mesh.Draw();
      }

      ///// XYZ軸モデル /////

      foreach (ModelMesh mesh in _xyz.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          // ライトの設定
          effect.EnableDefaultLighting();

          // 原点に配置
          effect.World = Matrix.Identity;

          // ビューマトリックスを設定
          effect.View = view;

          // プロジェクションマトリックス設定
          effect.Projection = projection;
        }

        mesh.Draw();
      }

      // スプライトの描画準備
      _spriteBatch.Begin();

      // カメラの情報を表示
      _spriteBatch.DrawString(_font,
          "CameraPosition : {" +
              _cameraPosition.X.ToString("f2") + ", " +
              _cameraPosition.Y.ToString("f2") + "}" + Environment.NewLine +
          "CameraTarget : {" +
              _cameraTarget.X.ToString("f2") + ", " +
              _cameraTarget.Y.ToString("f2") + "}" + Environment.NewLine +
          "CameraUpVectorRotate : " + _cameraUpVectorRotate.ToString("f2") + Environment.NewLine +
          "MousePressAutoMode : " + _autoMode,
          new Vector2(20, 20), Color.White);

      // スプライトの一括描画
      _spriteBatch.End();

      // 登録された DrawableGameComponent を描画する
      base.Draw(gameTime);
    }
  }
}
