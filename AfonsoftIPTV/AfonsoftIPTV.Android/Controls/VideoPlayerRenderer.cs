using System;
using System.ComponentModel;
using Android.Content;
using Android.Widget;
using ARelativeLayout = Android.Widget.RelativeLayout;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Math = System.Math;
using Android.Media;
using Afonsoft.Video.Controls;
using Afonsoft.Video;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(AfonsoftIPTV.Droid.Controls.VideoPlayerRenderer))]
namespace AfonsoftIPTV.Droid.Controls
{
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, ARelativeLayout>
    {
        VideoView videoView;
        MediaController mediaController;    // Used to display transport controls
        private bool isPrepared;
        private bool hasSetSource;

        public VideoPlayerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    // Save the VideoView for future reference
                    videoView = new VideoView(Context);

                    // Put the VideoView in a RelativeLayout
                    ARelativeLayout relativeLayout = new ARelativeLayout(Context);
                    relativeLayout.AddView(videoView);

                    // Center the VideoView in the RelativeLayout
                    ARelativeLayout.LayoutParams layoutParams =
                        new ARelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

                    layoutParams.AddRule(LayoutRules.CenterInParent);
                    videoView.LayoutParameters = layoutParams;
                    // Handle a VideoView event
                    videoView.Prepared += OnVideoViewPrepared;
                    videoView.AccessibilityTraversalBefore = 0;

                    //Error Handle
                    videoView.Error += OnError;

                    SetNativeControl(relativeLayout);


                }

                SetAreTransportControlsEnabled();
                SetSource();

                args.NewElement.UpdateStatus += OnUpdateStatus;
                args.NewElement.PlayRequested += OnPlayRequested;
                args.NewElement.PauseRequested += OnPauseRequested;
                args.NewElement.StopRequested += OnStopRequested;
            }

            if (args.OldElement != null)
            {
                args.OldElement.UpdateStatus -= OnUpdateStatus;
                args.OldElement.PlayRequested -= OnPlayRequested;
                args.OldElement.PauseRequested -= OnPauseRequested;
                args.OldElement.StopRequested -= OnStopRequested;
            }
        }

        private void OnError(object sender, MediaPlayer.ErrorEventArgs e)
        {
            isPrepared = false;
            videoView.StopPlayback();
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null && videoView != null)
            {
                videoView.Prepared -= OnVideoViewPrepared;
            }
            if (Element != null)
            {
                Element.UpdateStatus -= OnUpdateStatus;
            }

            base.Dispose(disposing);
        }

        void OnVideoViewPrepared(object sender, EventArgs args)
        {
            isPrepared = true;
            ((IVideoPlayerController)Element).Duration = TimeSpan.FromMilliseconds(videoView.Duration);

            //mediaPlayer = sender as MediaPlayer;

            //int videoWidth = mediaPlayer.VideoWidth;
            //int videoHeight = mediaPlayer.VideoHeight;

            //DisplayMetrics displayMetrics = new DisplayMetrics();
            //IWindowManager windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            //windowManager.DefaultDisplay.GetMetrics(displayMetrics);
            //int screenWidth = displayMetrics.WidthPixels;
            //int screenHeight = displayMetrics.HeightPixels;

            //float scaleY = 1.0f;
            //float scaleX = (videoWidth * screenHeight / videoHeight) / screenWidth;

            //int pivotPointX = (int)(screenWidth / 2);
            //int pivotPointY = (int)(screenHeight / 2);

            //surfaceView.setScaleX(scaleX);
            //surfaceView.setScaleY(scaleY);
            //surfaceView.setPivotX(pivotPointX);
            //surfaceView.setPivotY(pivotPointY);

            if (Element.AutoPlay && hasSetSource)
            {
                if (videoView.IsPlaying)
                    videoView.StopPlayback();
                videoView.RequestFocus();
                videoView.Start();
                videoView.SeekTo(0);
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == VideoPlayer.AreTransportControlsEnabledProperty.PropertyName)
            {
                SetAreTransportControlsEnabled();
            }
            else if (args.PropertyName == VideoPlayer.SourceProperty.PropertyName)
            {
                SetSource();
            }
            else if (args.PropertyName == VideoPlayer.PositionProperty.PropertyName)
            {
                if (Math.Abs(videoView.CurrentPosition - Element.Position.TotalMilliseconds) > 1000)
                {
                    videoView.SeekTo((int)Element.Position.TotalMilliseconds);
                }
            }
        }

        void SetAreTransportControlsEnabled()
        {
            if (Element.AreTransportControlsEnabled)
            {
                mediaController = new MediaController(Context);
                mediaController.SetMediaPlayer(videoView);
                videoView.SetMediaController(mediaController);
            }
            else
            {
                videoView.SetMediaController(null);
                if (mediaController != null)
                {
                    mediaController.SetMediaPlayer(null);
                    mediaController = null;
                }
            }
        }

        void SetSource()
        {
            isPrepared = false;
            hasSetSource = false;
            videoView.Activated = true;
            videoView.StopPlayback();

            if (Element.Source is UriVideoSource source)
            {
                string uri = source.Uri;

                if (!string.IsNullOrWhiteSpace(uri))
                {
                    videoView.SetVideoURI(Android.Net.Uri.Parse(uri));
                    hasSetSource = true;
                }
            }
            else if (Element.Source is FileVideoSource videoSource)
            {
                string filename = videoSource.File;

                if (!string.IsNullOrWhiteSpace(filename))
                {
                    videoView.SetVideoPath(filename);
                    hasSetSource = true;
                }
            }
        }

        // Event handler to update status
        void OnUpdateStatus(object sender, EventArgs args)
        {
            VideoStatus status = VideoStatus.NotReady;

            if (isPrepared)
            {
                status = videoView.IsPlaying ? VideoStatus.Playing : VideoStatus.Paused;
            }

            ((IVideoPlayerController)Element).Status = status;

            // Set Position property
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(videoView.CurrentPosition);
            ((IElementController)Element).SetValueFromRenderer(VideoPlayer.PositionProperty, timeSpan);
        }

        // Event handlers to implement methods
        void OnPlayRequested(object sender, EventArgs args)
        {
            if (videoView.IsPlaying)
                videoView.StopPlayback();
            videoView.RequestFocus();
            videoView.Start();
        }

        void OnPauseRequested(object sender, EventArgs args)
        {
            videoView.Pause();
        }

        void OnStopRequested(object sender, EventArgs args)
        {
            videoView.StopPlayback();
        }
    }
}