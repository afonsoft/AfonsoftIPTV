﻿using System;
using System.Collections.Generic;
using System.Text;
using Afonsoft.Video;
using Xamarin.Forms;

namespace Afonsoft.Video.Controls
{
    public class VideoPlayer : View, IVideoPlayerController
    {
        public event EventHandler OnCompletion;
        public event EventHandler UpdateStatus;
        private int indexPlayList;

        public VideoPlayer()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                UpdateStatus?.Invoke(this, EventArgs.Empty);
                return true;
            });
        }

        // AreTransportControlsEnabled property
        public static readonly BindableProperty AreTransportControlsEnabledProperty =
            BindableProperty.Create(nameof(AreTransportControlsEnabled), typeof(bool), typeof(VideoPlayer), true);

        public bool AreTransportControlsEnabled
        {
            set => SetValue(AreTransportControlsEnabledProperty, value);
            get => (bool)GetValue(AreTransportControlsEnabledProperty);
        }

        // Source property
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(VideoSource), typeof(VideoPlayer));

        [TypeConverter(typeof(VideoSourceConverter))]
        public VideoSource Source
        {
            set => SetValue(SourceProperty, value);
            get => (VideoSource)GetValue(SourceProperty);
        }


        // Source property
        public static readonly BindableProperty PlayListProperty =
            BindableProperty.Create(nameof(PlayList), typeof(List<VideoSource>), typeof(VideoPlayer));

        [TypeConverter(typeof(List<VideoSourceConverter>))]
        public List<VideoSource> PlayList
        {
            set => SetValue(PlayListProperty, value);
            get => (List<VideoSource>)GetValue(PlayListProperty);
        }

        // AutoPlay property
        public static readonly BindableProperty AutoPlayProperty =
            BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(VideoPlayer), true);

        public bool AutoPlay
        {
            set => SetValue(AutoPlayProperty, value);
            get => (bool)GetValue(AutoPlayProperty);
        }

        // Status read-only property
        private static readonly BindablePropertyKey StatusPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Status), typeof(VideoStatus), typeof(VideoPlayer), VideoStatus.NotReady);

        public static readonly BindableProperty StatusProperty = StatusPropertyKey.BindableProperty;

        public VideoStatus Status => (VideoStatus)GetValue(StatusProperty);

        VideoStatus IVideoPlayerController.Status
        {
            set => SetValue(StatusPropertyKey, value);
            get => Status;
        }

        // Duration read-only property
        private static readonly BindablePropertyKey DurationPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Duration), typeof(TimeSpan), typeof(VideoPlayer), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((VideoPlayer)bindable).SetTimeToEnd());

        public static readonly BindableProperty DurationProperty = DurationPropertyKey.BindableProperty;

        public TimeSpan Duration => (TimeSpan)GetValue(DurationProperty);

        TimeSpan IVideoPlayerController.Duration
        {
            set => SetValue(DurationPropertyKey, value);
            get => Duration;
        }

        // Position property
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(TimeSpan), typeof(VideoPlayer), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((VideoPlayer)bindable).SetTimeToEnd());

        public TimeSpan Position
        {
            set => SetValue(PositionProperty, value);
            get => (TimeSpan)GetValue(PositionProperty);
        }

        // TimeToEnd property
        private static readonly BindablePropertyKey TimeToEndPropertyKey =
            BindableProperty.CreateReadOnly(nameof(TimeToEnd), typeof(TimeSpan), typeof(VideoPlayer), new TimeSpan());

        public static readonly BindableProperty TimeToEndProperty = TimeToEndPropertyKey.BindableProperty;

        public TimeSpan TimeToEnd
        {
            private set => SetValue(TimeToEndPropertyKey, value);
            get => (TimeSpan)GetValue(TimeToEndProperty);
        }

        void SetTimeToEnd()
        {
            TimeToEnd = Duration - Position;
            if (TimeToEnd <= TimeSpan.FromMilliseconds(500) && Duration != TimeSpan.Zero ||
                (TimeToEnd == Duration && Status == VideoStatus.Paused))
                OnVideoCompletion(this, EventArgs.Empty);
        }

        private void OnVideoCompletion(object sender, EventArgs args)
        {
            if (PlayList != null && PlayList.Count > 0)
            {
                indexPlayList = (indexPlayList + 1) % PlayList.Count;
                Source = PlayList[indexPlayList];
                if (AutoPlay)
                {
                    Stop();
                    Play();
                }

            }

            OnCompletion?.Invoke(sender, args);
        }

        // Methods handled by renderers
        public event EventHandler PlayRequested;

        public void Play()
        {
            PlayRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler PauseRequested;

        public void Pause()
        {
            PauseRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler StopRequested;

        public void Stop()
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}