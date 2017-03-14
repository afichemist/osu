﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK.Input;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Overlays.Mods;
using osu.Game.Screens.Play;

namespace osu.Game.Screens.Select
{
    public class PlaySongSelect : SongSelect
    {
        private OsuScreen player;
        private ModSelectOverlay modSelect;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Add(modSelect = new ModSelectOverlay
            {
                RelativeSizeAxes = Axes.X,
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Margin = new MarginPadding { Bottom = 50 }
            });

            Footer.AddButton(@"mods", colours.Yellow, modSelect.ToggleVisibility, Key.F1);
            Footer.AddButton(@"random", colours.Green, SelectRandom, Key.F2);
            Footer.AddButton(@"options", colours.Blue, BeatmapOptions.ToggleVisibility, Key.F3);
        }

        protected override void OnBeatmapChanged(WorkingBeatmap beatmap)
        {
            beatmap?.Mods.BindTo(modSelect.SelectedMods);
            base.OnBeatmapChanged(beatmap);
        }

        protected override void OnResuming(Screen last)
        {
            player = null;
            base.OnResuming(last);
        }

        protected override void OnSelected(WorkingBeatmap beatmap)
        {
            if (player != null) return;

            (player = new PlayerLoader(new Player
            {
                Beatmap = Beatmap, //eagerly set this so it's present before push.
            })).LoadAsync(Game, l => Push(player));
        }
    }
}
